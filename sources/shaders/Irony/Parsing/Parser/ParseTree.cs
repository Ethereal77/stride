// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2011 Irony - Roman Ivantsov
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stride.Irony.Parsing {
  /*
    A node for a parse tree (concrete syntax tree) - an initial syntax representation produced by parser.
    It contains all syntax elements of the input text, each element represented by a generic node ParseTreeNode.
    The parse tree is converted into abstract syntax tree (AST) which contains custom nodes. The conversion might
    happen on-the-fly: as parser creates the parse tree nodes it can create the AST nodes and puts them into AstNode field.
    Alternatively it might happen as a separate step, after completing the parse tree.
    AST node might optinally implement IAstNodeInit interface, so Irony parser can initialize the node providing it
    with all relevant information.
    The ParseTreeNode also works as a stack element in the parser stack, so it has the State property to carry
    the pushed parser state while it is in the stack.
  */
  public class ParseTreeNode {
    public object AstNode;
    public Token Token;
    public BnfTerm Term;
    public int Precedence;
    public Associativity Associativity;
    public SourceSpan Span;
    public Production ReduceProduction;
    //Making ChildNodes property (not field) following request by Matt K, Bill H
    public ParseTreeNodeList ChildNodes {get; private set;}
    public bool IsError;
    internal ParserState State;      //used by parser to store current state when node is pushed into the parser stack
    public object Tag; //for use by custom parsers, Irony does not use it

    private ParseTreeNode(){
      ChildNodes = new ParseTreeNodeList();
    }
    public ParseTreeNode(BnfTerm term) : this() {
      Term = term;
    }

    public ParseTreeNode(Token token) : this()  {
      Token = token;
      Term = token.Terminal;
      Precedence = Term.Precedence;
      Associativity = token.Terminal.Associativity;
      Span = new SourceSpan(token.Location, token.Length);
      IsError = token.IsError();
    }

    public ParseTreeNode(ParserState initialState) : this() {
      State = initialState;
    }

    public ParseTreeNode(Production reduceProduction, SourceSpan span)  : this(){
      ReduceProduction = reduceProduction;
      Span = span;
      Term = ReduceProduction.LValue;
      Precedence = Term.Precedence;
    }

    public override string ToString() {
      if (Term == null)
        return "(S0)"; //initial state node
      else
        return Term.GetParseNodeCaption(this);
    }//method

    private static Token FindFirstChildTokenRec(ParseTreeNode node) {
      if (node.Token != null) return node.Token;
      foreach (var child in node.ChildNodes) {
        var tkn = FindFirstChildTokenRec(child);
        if (tkn != null) return tkn;
      }
      return null;
    }

  }//class

  public class ParseTreeNodeList : List<ParseTreeNode> { }

  public enum ParseTreeStatus {
    Parsing,
    Partial,
    Parsed,
    Error,
  }

  public class ParseTree {
    public ParseTreeStatus Status {get; internal set;}
    public readonly string SourceText;
    public readonly string FileName;
    public readonly TokenList Tokens = new TokenList();
    public readonly TokenList OpenBraces = new TokenList();
    public ParseTreeNode Root;
    public readonly ParserMessageList ParserMessages = new ParserMessageList();
    public int ParseTime;

    public ParseTree(string sourceText, string fileName) {
      SourceText = sourceText;
      FileName = fileName;
      Status = ParseTreeStatus.Parsing;
    }

    public bool HasErrors() {
      if (ParserMessages.Count == 0) return false;
      foreach (var err in ParserMessages)
        if (err.Level == ParserErrorLevel.Error) return true;
      return false;
    }//method

  }//class

}
