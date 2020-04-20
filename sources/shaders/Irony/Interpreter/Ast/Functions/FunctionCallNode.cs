// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
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

  //A node representing function call
  public class FunctionCallNode : AstNode {
    AstNode TargetRef;
    AstNode Arguments;
    string _targetName;
     
    public override void Init(ParsingContext context, ParseTreeNode treeNode) {
      base.Init(context, treeNode);
      TargetRef = AddChild("Target", treeNode.ChildNodes[0]);
      _targetName = treeNode.ChildNodes[0].FindTokenAndGetText(); 
      Arguments = AddChild("Args", treeNode.ChildNodes[1]);
      AsString = "Call " + _targetName;
    }
    
    public override void EvaluateNode(EvaluationContext context, AstMode mode) {
      TargetRef.Evaluate(context, AstMode.Read);
      var target = context.Data.Pop() as ICallTarget;
      if (target == null)
        context.ThrowError(Resources.ErrVarIsNotCallable, _targetName);
      Arguments.Evaluate(context, AstMode.Read);
      target.Call(context);
    }

  }//class

}//namespace
