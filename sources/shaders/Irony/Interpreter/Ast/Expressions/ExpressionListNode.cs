// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2011 Irony - Roman Ivantsov
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Irony.Interpreter;
using Irony.Parsing;

namespace Irony.Interpreter.Ast {

  //A node representing expression list - for example, list of argument expressions in function call
  public class ExpressionListNode : AstNode {
     
    public override void Init(ParsingContext context, ParseTreeNode treeNode) {
      base.Init(context, treeNode);
      foreach (var child in treeNode.ChildNodes) {
          AddChild("expr", child); 
      }
      AsString = "Expression list";
    }

    public override void EvaluateNode(EvaluationContext context, AstMode mode) {
      var result = new ValuesList();
      foreach (var expr in ChildNodes) {
        expr.Evaluate(context, AstMode.Read);
        result.Add(context.Data.Pop());
      }
      //Push list on the stack
      context.Data.Push(result); 
    }

  }//class

}//namespace
