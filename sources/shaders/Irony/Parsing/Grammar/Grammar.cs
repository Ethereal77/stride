// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2011 Irony - Roman Ivantsov
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Stride.Irony.Parsing {

  public partial class Grammar {

    #region properties
    /// <summary>
    /// Gets case sensitivity of the grammar. Read-only, true by default.
    /// Can be set to false only through a parameter to grammar constructor.
    /// </summary>
    public readonly bool CaseSensitive = true;
    public readonly StringComparer LanguageStringComparer;

    //List of chars that unambigously identify the start of new token.
    //used in scanner error recovery, and in quick parse path in NumberLiterals, Identifiers
    public string Delimiters = null;

    public string WhitespaceChars = " \t\r\n\v";

    //Used for line counting in source file
    public string LineTerminators = "\n\r\v";

    #region Language Flags
    public LanguageFlags LanguageFlags = LanguageFlags.Default;

    public bool FlagIsSet(LanguageFlags flag) {
      return (LanguageFlags & flag) != 0;
    }

    public TermReportGroupList TermReportGroups = new TermReportGroupList();
    #endregion

    //Terminals not present in grammar expressions and not reachable from the Root
    // (Comment terminal is usually one of them)
    // Tokens produced by these terminals will be ignored by parser input.
    public readonly TerminalSet NonGrammarTerminals = new TerminalSet();

    //Terminals that either don't have explicitly declared Firsts symbols, or can start with chars not covered by these Firsts
    // For ex., identifier in c# can start with a Unicode char in one of several Unicode classes, not necessarily latin letter.
    //  Whenever terminals with explicit Firsts() cannot produce a token, the Scanner would call terminals from this fallback
    // collection to see if they can produce it.
    // Note that IdentifierTerminal automatically add itself to this collection if its StartCharCategories list is not empty,
    // so programmer does not need to do this explicitly
    public readonly TerminalSet FallbackTerminals = new TerminalSet();

    public Type DefaultNodeType;


    /// <summary>
    /// The main root entry for the grammar.
    /// </summary>
    public NonTerminal Root;

    public Func<Scanner> ScannerBuilder;

      /// <summary>
    /// Alternative roots for parsing code snippets.
    /// </summary>
    public NonTerminalSet SnippetRoots = new NonTerminalSet();

    public string GrammarComments; //shown in Grammar info tab

    public CultureInfo DefaultCulture = CultureInfo.InvariantCulture;

    #endregion

    #region constructors

    public virtual LanguageData CreateLanguageData()
    {
        return new LanguageData(this);
    }

    public Grammar() : this(true) { } //case sensitive by default

    public Grammar(bool caseSensitive) {
      _currentGrammar = this;
      this.CaseSensitive = caseSensitive;
      LanguageStringComparer = caseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase;
      KeyTerms = new KeyTermTable(LanguageStringComparer);
    }
    #endregion

    #region Register/Mark methods
    public void MarkPunctuation(params string[] symbols) {
      foreach (string symbol in symbols) {
        KeyTerm term = ToTerm(symbol);
        term.SetFlag(TermFlags.IsPunctuation|TermFlags.NoAstNode);
      }
    }
    #endregion

    #region virtual methods: TryMatch, CreateNode, CreateRuntime, RunSample
    public virtual void CreateTokenFilters(LanguageData language, TokenFilterList filters) {
    }

    //Gives a way to customize parse tree nodes captions in the tree view.
    public virtual string GetParseNodeCaption(ParseTreeNode node) {
      if (node.IsError)
        return node.Term.Name + " (Syntax error)";
      if (node.Token != null)
        return node.Token.ToString();
      if (node.Term == null) //special case for initial node pushed into the stack at parser start
        return node.State != null ? "(State " + node.State.Name + ")" : string.Empty; //  Resources.LabelInitialState;
      var ntTerm = node.Term as NonTerminal;
      if (ntTerm != null && !string.IsNullOrEmpty(ntTerm.NodeCaptionTemplate))
        return ntTerm.GetNodeCaption(node);
      return node.Term.Name;
    }

    //Gives a chance of custom AST node creation at Grammar level
    // by default calls Term's method
    public virtual void CreateAstNode(ParsingContext context, ParseTreeNode nodeInfo) {
      nodeInfo.Term.CreateAstNode(context, nodeInfo);
    }

    /// <summary>
    /// Override this method to provide custom conflict resolution; for example, custom code may decide proper shift or reduce
    /// action based on preview of tokens ahead.
    /// </summary>
    public virtual void OnResolvingConflict(ConflictResolutionArgs args) {
      //args.Result is Shift by default
    }

    //The method is called after GrammarData is constructed
    public virtual void OnGrammarDataConstructed(LanguageData language) {
    }

    public virtual void OnLanguageDataConstructed(LanguageData language) {
    }


    //Constructs the error message in situation when parser has no available action for current input.
    // override this method if you want to change this message
    public virtual string ConstructParserErrorMessage(ParsingContext context, StringSet expectedTerms) {
      return string.Format(Resources.ErrParserUnexpInput, expectedTerms.ToString(" "));
    }

    // Override this method to perform custom error processing
    public virtual void ReportParseError(ParsingContext context) {
        string error = null;
        if (context.CurrentParserInput.Term == this.SyntaxError)
            error = context.CurrentParserInput.Token.Value as string; //scanner error
        else if (context.CurrentParserInput.Term == this.Indent)
            error = Resources.ErrUnexpIndent;
        else if (context.CurrentParserInput.Term == this.Eof && context.OpenBraces.Count > 0) {
            //report unclosed braces/parenthesis
            var openBrace = context.OpenBraces.Peek();
            error = string.Format(Resources.ErrNoClosingBrace, openBrace.Text);
        } else {
            var expectedTerms = context.GetExpectedTermSet();
            if (expectedTerms.Count > 0)
              error = ConstructParserErrorMessage(context, expectedTerms);
              //error = string.Format(Resources.ErrParserUnexpInput, expectedTerms.ToString(" ")
            else
              error = Resources.ErrUnexpEof;
        }
        context.AddParserError(error);
    }//method

    #endregion

    #region MakePlusRule, MakeStarRule methods
    public static BnfExpression MakePlusRule(NonTerminal listNonTerminal, BnfTerm listMember) {
      return MakePlusRule(listNonTerminal, null, listMember);
    }

    public static BnfExpression MakePlusRule(NonTerminal listNonTerminal, BnfTerm delimiter, BnfTerm listMember) {
      if (delimiter == null)
        listNonTerminal.Rule = listMember | listNonTerminal + listMember;
      else
        listNonTerminal.Rule = listMember | listNonTerminal + delimiter + listMember;
      listNonTerminal.SetFlag(TermFlags.IsList);
      return listNonTerminal.Rule;
    }

    public static BnfExpression MakeStarRule(NonTerminal listNonTerminal, BnfTerm listMember) {
      return MakeStarRule(listNonTerminal, null, listMember, TermListOptions.None);
    }

    public static BnfExpression MakeStarRule(NonTerminal listNonTerminal, BnfTerm delimiter, BnfTerm listMember) {
      return MakeStarRule(listNonTerminal, delimiter, listMember, TermListOptions.None);
    }

    public static BnfExpression MakeStarRule(NonTerminal listNonTerminal, BnfTerm delimiter, BnfTerm listMember, TermListOptions options) {
       bool allowTrailingDelimiter = (options & TermListOptions.AllowTrailingDelimiter) != 0;
      if (delimiter == null) {
        //it is much simpler case
        listNonTerminal.SetFlag(TermFlags.IsList);
        listNonTerminal.Rule = _currentGrammar.Empty | listNonTerminal + listMember;
        return listNonTerminal.Rule;
      }
      //Note that deceptively simple version of the star-rule
      //       Elem* -> Empty | Elem | Elem* + delim + Elem
      //  does not work when you have delimiters. This simple version allows lists starting with delimiters -
      // which is wrong. The correct formula is to first define "Elem+"-list, and then define "Elem*" list
      // as "Elem* -> Empty|Elem+"
      NonTerminal plusList = new NonTerminal(listMember.Name + "+");
      plusList.Rule = MakePlusRule(plusList, delimiter, listMember);
      plusList.SetFlag(TermFlags.NoAstNode); //to allow it to have AstNodeType not assigned
      if (allowTrailingDelimiter)
        listNonTerminal.Rule = _currentGrammar.Empty | plusList | plusList + delimiter;
      else
        listNonTerminal.Rule = _currentGrammar.Empty | plusList;
      listNonTerminal.SetFlag(TermFlags.IsListContainer);
      return listNonTerminal.Rule;
    }
    #endregion

    #region Hint utilities
    protected GrammarHint PreferShiftHere() {
      return new GrammarHint(HintType.ResolveToShift, null);
    }
    protected GrammarHint ReduceHere() {
      return new GrammarHint(HintType.ResolveToReduce, null);
    }

    #endregion

    #region Standard terminals: EOF, Empty, NewLine, Indent, Dedent
    // Empty object is used to identify optional element:
    //    term.Rule = term1 | Empty;
    public readonly Terminal Empty = new Terminal("EMPTY");
    // The following terminals are used in indent-sensitive languages like Python;
    // they are not produced by scanner but are produced by CodeOutlineFilter after scanning
    public readonly Terminal NewLine = new Terminal("LF");
    public readonly Terminal Indent = new Terminal("INDENT", TokenCategory.Outline, TermFlags.IsNonScanner);
    public readonly Terminal Dedent = new Terminal("DEDENT", TokenCategory.Outline, TermFlags.IsNonScanner);
    //End-of-Statement terminal - used in indentation-sensitive language to signal end-of-statement;
    // it is not always synced with CRLF chars, and CodeOutlineFilter carefully produces Eos tokens
    // (as well as Indent and Dedent) based on line/col information in incoming content tokens.
    public readonly Terminal Eos = new Terminal("EOS", Resources.LabelEosLabel, TokenCategory.Outline, TermFlags.IsNonScanner);
    // Identifies end of file
    // Note: using Eof in grammar rules is optional. Parser automatically adds this symbol
    // as a lookahead to Root non-terminal
    public readonly Terminal Eof = new Terminal("EOF", TokenCategory.Outline);

    //Used for error tokens
    public readonly Terminal LineStartTerminal = new Terminal("LINE_START", TokenCategory.Outline);

    //Used for error tokens
    public readonly Terminal SyntaxError = new Terminal("SYNTAX_ERROR", TokenCategory.Error, TermFlags.IsNonScanner);

    #endregion

    #region KeyTerms (keywords + special symbols)
    public KeyTermTable KeyTerms;

    public KeyTerm ToTerm(string text) {
      return ToTerm(text, text);
    }
    public KeyTerm ToTerm(string text, string name) {
      KeyTerm term;
      if (KeyTerms.TryGetValue(text, out term)) {
        //update name if it was specified now and not before
        if (string.IsNullOrEmpty(term.Name) && !string.IsNullOrEmpty(name))
          term.Name = name;
        return term;
      }
      //create new term
      if (!CaseSensitive)
        text = text.ToLowerInvariant();
      text = string.Intern(text);
      term = new KeyTerm(text, name);
      KeyTerms[text] = term;
      return term;
    }

    #endregion

    #region CurrentGrammar static field
    //Static per-thread instance; Grammar constructor sets it to self (this).
    // This field/property is used by operator overloads (which are static) to access Grammar's predefined terminals like Empty,
    //  and SymbolTerms dictionary to convert string literals to symbol terminals and add them to the SymbolTerms dictionary
    [ThreadStatic]
    private static Grammar _currentGrammar;
    public static Grammar CurrentGrammar {
      get { return _currentGrammar; }
    }
    #endregion

  }//class

}//namespace
