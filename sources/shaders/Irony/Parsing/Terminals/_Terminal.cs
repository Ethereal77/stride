// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2011 Irony - Roman Ivantsov
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Stride.Irony.Parsing {

  public class Terminal : BnfTerm {
    #region Constructors
    public Terminal(string name)  : this(name, TokenCategory.Content, TermFlags.None) {  }
    public Terminal(string name, TokenCategory category)  : this(name, category, TermFlags.None) {  }
    public Terminal(string name, string errorAlias, TokenCategory category, TermFlags flags) : this(name, category, flags) {
      this.ErrorAlias = errorAlias;
    }
    public Terminal(string name, TokenCategory category, TermFlags flags)  : base(name) {
      Category = category;
      this.Flags |= flags;
      if (Category == TokenCategory.Outline)
        this.SetFlag(TermFlags.IsPunctuation);
      OutputTerminal = this;
    }
    #endregion

    #region fields and properties
    public TokenCategory Category = TokenCategory.Content;
    // Priority is used when more than one terminal may match the input char.
    // It determines the order in which terminals will try to match input for a given char in the input.
    // For a given input char the scanner uses the hash table to look up the collection of terminals that may match this input symbol.
    // It is the order in this collection that is determined by Priority property - the higher the priority,
    // the earlier the terminal gets a chance to check the input.
    public int Priority; //default is 0

    //Terminal to attach to the output token. By default is set to the Terminal itself
    // Use SetOutputTerminal method to change it. For example of use see TerminalFactory.CreateSqlIdentifier and sample SQL grammar
    public Terminal OutputTerminal { get; private set; }

    public TokenEditorInfo EditorInfo;
    public byte MultilineIndex;
    public Terminal IsPairFor;
    #endregion

    #region virtual methods: GetFirsts(), TryMatch, Init, TokenToString

    //"Firsts" (chars) collections are used for quick search for possible matching terminal(s) using current character in the input stream.
    // A terminal might declare no firsts. In this case, the terminal is tried for match for any current input character.
    public virtual IList<string> GetFirsts() {
      return null;
    }

    public virtual Token TryMatch(ParsingContext context, ISourceStream source) {
      return null;
    }

    public virtual string TokenToString(Token token) {
      if (token.ValueString == this.Name)
        return token.ValueString;
      else
        return (token.ValueString ?? token.Text) + " (" + Name + ")";
    }


    #endregion

    #region Events: ValidateToken
    public event EventHandler<ParsingEventArgs> ValidateToken;
    #endregion

    #region static comparison methods
    public static int ByName(Terminal x, Terminal y) {
      return string.Compare(x.ToString(), y.ToString());
    }
    #endregion

    //Priority constants
    public const int LowestPriority = -1000;
    public const int HighestPriority = 1000;
    public const int ReservedWordsPriority = 900; //almost top one

    public static string TerminalsToString(IEnumerable<Terminal> terminals, string separator) {
      var sb = new StringBuilder();
      foreach (var term in terminals) {
        sb.Append(term.ToString());
        sb.Append(separator);
      }
      return sb.ToString().Trim();
    }

  }//class

  public class TerminalSet : HashSet<Terminal> {
    public override string ToString() {
      return Terminal.TerminalsToString(this, " ");
    }
  }

  //No-duplicates list of terminals
  public class TerminalList : List<Terminal> {
    public new void Add(Terminal terminal) {
      if (!Contains(terminal))
        base.Add(terminal);
    }
    public override string ToString() {
      return Terminal.TerminalsToString(this, " ");
    }
  }


}//namespace
