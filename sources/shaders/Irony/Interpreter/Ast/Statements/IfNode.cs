// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2011 Irony - Roman Ivantsov
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

using Irony.Interpreter;
using Irony.Parsing;

namespace Irony.Interpreter.Ast {
  public class IfNode : AstNode {
    public AstNode Test;
    public AstNode IfTrue;
    public AstNode IfFalse;

    public IfNode() { }

    public override void Init(ParsingContext context, ParseTreeNode treeNode) {
      base.Init(context, treeNode);
      Test = AddChild("Test", treeNode.ChildNodes[0]);
      IfTrue = AddChild("IfTrue", treeNode.ChildNodes[1]);
      if (treeNode.ChildNodes.Count > 2)
        IfFalse = AddChild("IfFalse", treeNode.ChildNodes[2]);
    } 

    public override void EvaluateNode(EvaluationContext context, AstMode mode) {
      Test.Evaluate(context, AstMode.Write);
      var result = context.Data.Pop();
      if (context.Runtime.IsTrue(result)) {
        if (IfTrue != null)    IfTrue.Evaluate(context, AstMode.Read);
      } else {
        if (IfFalse != null)   IfFalse.Evaluate(context, AstMode.Read);
      }
    }
  }//class

}//namespace
