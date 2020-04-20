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
  public class BlockNode : StatementListNode {

    public BlockNode() { }
    public override void Init(ParsingContext context, ParseTreeNode treeNode) {
      base.Init(context, treeNode.ChildNodes[0]);
      AsString = "Block";
    }


  }//class
}//namespace
