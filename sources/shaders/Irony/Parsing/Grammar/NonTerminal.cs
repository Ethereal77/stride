// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2011 Irony - Roman Ivantsov
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Stride.Irony.Parsing {

  internal class IntList : List<int> { }

  public class NonTerminal : BnfTerm {

    #region constructors
    public NonTerminal(string name)  : base(name, null) { }  //by default display name is null
    public NonTerminal(string name, AstNodeCreator nodeCreator) : base(name, null, nodeCreator) { }
    public NonTerminal(string name, BnfExpression expression)
      : this(name) {
      Rule = expression;
    }
    #endregion

    #region properties/fields: Rule, ErrorRule

    public BnfExpression Rule;
    //Separate property for specifying error expressions. This allows putting all such expressions in a separate section
    // in grammar for all non-terminals. However you can still put error expressions in the main Rule property, just like
    // in YACC
    public BnfExpression ErrorRule;

    //A template for representing ParseTreeNode in the parse tree. Can contain '#{i}' fragments referencing
    // child nodes by index
    public string NodeCaptionTemplate;
    //Converted template with index list
    private string _convertedTemplate;
    private IntList _captionParameters;
    #endregion

    #region overrides: ToString, Init
    public override string ToString() {
      return Name;
    }
    public override void Init(GrammarData grammarData) {
      base.Init(grammarData);
      if (!string.IsNullOrEmpty(NodeCaptionTemplate))
        ConvertNodeCaptionTemplate();
      if (TokenPreviewHint != null)
        TokenPreviewHint.Init(grammarData);
    }
    #endregion

    #region data used by Parser builder
    public readonly ProductionList Productions = new ProductionList();
    #endregion

    #region custom grammar hints
    TokenPreviewHint TokenPreviewHint { get; set; }
    internal void InsertCustomHints() {
      if (TokenPreviewHint != null && Productions.Count > 0) {
        foreach (var production in Productions) {
          foreach (var lr0item in production.LR0Items) {
            lr0item.Hints.Add(TokenPreviewHint);
          }
        }
      }
    }
    #endregion

    public static string NonTerminalsToString(IEnumerable<NonTerminal> terms, string separator) {
      var sb = new StringBuilder();
      foreach (var term in terms) {
        sb.Append(term.ToString());
        sb.Append(separator);
      }
      return sb.ToString().Trim();
    }

    #region NodeCaptionTemplate utilities
    //We replace original tag '#{i}'  (where i is the index of the child node to put here)
    // with the tag '{k}', where k is the number of the parameter. So after conversion the template can
    // be used in string.Format() call, with parameters set to child nodes captions
    private void ConvertNodeCaptionTemplate() {
      _captionParameters = new IntList();
      _convertedTemplate = NodeCaptionTemplate;
      var index = 0;
      while(index < 100) {
        var strParam = "#{" + index + "}";
        if (_convertedTemplate.Contains(strParam)) {
          _convertedTemplate = _convertedTemplate.Replace(strParam, "{" + _captionParameters.Count + "}");
          _captionParameters.Add(index);
        }
        if (!_convertedTemplate.Contains("#{")) return;
        index++;
      }//while
    }//method

    public string GetNodeCaption(ParseTreeNode node) {
      var paramValues = new string[_captionParameters.Count];
      for(int i = 0; i < _captionParameters.Count; i++) {
        var childIndex = _captionParameters[i];
        if (childIndex < node.ChildNodes.Count) {
          var child = node.ChildNodes[childIndex];
          //if child is a token, then child.ToString returns token.ToString which contains Value + Term;
          // in this case we prefer to have Value only
          paramValues[i] = (child.Token != null? child.Token.ValueString : child.ToString());
        }
      }
      var result = string.Format(_convertedTemplate, paramValues);
      return result;
    }
    #endregion

  }//class

  public class NonTerminalList : List<NonTerminal> {
    public override string ToString() {
      return NonTerminal.NonTerminalsToString(this, " ");
    }
  }

  public class NonTerminalSet : HashSet<NonTerminal> {
    public override string ToString() {
      return NonTerminal.NonTerminalsToString(this, " ");
    }
  }


}//namespace
