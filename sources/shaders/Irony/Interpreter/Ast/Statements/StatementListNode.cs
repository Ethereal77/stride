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

  public class StatementListNode : AstNode {
     
    public override void Init(ParsingContext context, ParseTreeNode treeNode) {
      base.Init(context, treeNode);
      foreach (var child in treeNode.ChildNodes) {
        //don't add if it is null; it can happen that "statement" is a comment line and statement's node is null.
        // So to make life easier for language creator, we just skip if it is null
        if (child.AstNode != null) 
          AddChild(string.Empty, child); 
      }
      AsString = "Statement List";
    }

    public override void EvaluateNode(EvaluationContext context, AstMode mode) {
      if (ChildNodes.Count == 0) return;
      ChildNodes[ChildNodes.Count - 1].Flags |= AstNodeFlags.IsTail;
      int iniCount = context.Data.Count; 
      foreach(var stmt in ChildNodes) {
        stmt.Evaluate(context, AstMode.Read);
        //restore position, in case a statement left something (like standalone expression vs assignment) 
        context.Data.PopUntil(iniCount);
      }
      context.Data.Push(context.LastResult); //push it back again
    }
    
  }//class

}//namespace
