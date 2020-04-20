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

  public class ParamListNode : AstNode {
     
    public override void Init(ParsingContext context, ParseTreeNode treeNode) {
      base.Init(context, treeNode);
      foreach (var child in treeNode.ChildNodes) {
          AddChild("parameter", child); 
      }
      AsString = "Param list";
    }

    public override void EvaluateNode(EvaluationContext context, AstMode mode) {
      var argsObj = context.Data.Pop();
      var args = argsObj as ValuesList;
      if (args == null)
        context.ThrowError(Resources.ErrArgListNotFound, argsObj);
      if (args.Count != ChildNodes.Count)
        context.ThrowError(Resources.ErrWrongArgCount, ChildNodes.Count, args.Count);

      for(int i = 0; i < ChildNodes.Count; i++) {
        context.Data.Push(args[i]);
        ChildNodes[i].Evaluate(context, AstMode.Write); 
      }
    }//method

  }//class

}//namespace
