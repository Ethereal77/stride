// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Irony.Parsing;
using Irony.Interpreter;

namespace Irony.Interpreter.Ast {
  public class LiteralValueNode : AstNode {
    public object Value; 

    public override void Init(ParsingContext context, ParseTreeNode treeNode) {
      base.Init(context, treeNode); 
      Value = treeNode.Token.Value;
      AsString = Value == null ? "null" : Value.ToString();
      if (Value is string)
        AsString = "\"" + AsString + "\""; 
    }

    public override void EvaluateNode(EvaluationContext context, AstMode mode) {
      switch (mode) {
        case AstMode.Read: 
          context.Data.Push(Value); 
          break;
        case AstMode.Write: 
          context.ThrowError(Resources.ErrAssignLiteralValue);  
          break;  
      }
    }
  
  }//class
}
