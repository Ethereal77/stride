// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2011 Irony - Roman Ivantsov
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using Irony.Parsing;
using Irony.Interpreter;

namespace Irony.Interpreter.Ast {

  public class IdentifierNode : AstNode {
    public string Symbol;

    public IdentifierNode() { }

    public override void Init(ParsingContext context, ParseTreeNode treeNode) {
      base.Init(context, treeNode);
      Symbol = treeNode.Token.ValueString;
      AsString = Symbol; 
    }


    public override void EvaluateNode(EvaluationContext context, AstMode mode) {
      switch (mode) {
        case AstMode.Read:
          object value;
          if (context.TryGetValue(Symbol, out value))
            context.Data.Push(value); 
          else 
            context.ThrowError(Resources.ErrVarNotDefined, Symbol);
          break; 
        case AstMode.Write:
          context.SetValue(Symbol, context.Data.Pop()); 
          break; 
      }
    }

  }//class
}//namespace
