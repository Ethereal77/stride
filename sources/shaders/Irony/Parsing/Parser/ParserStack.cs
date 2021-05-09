// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2011 Irony - Roman Ivantsov
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Stride.Irony.Parsing {

  public class ParserStack : List<ParseTreeNode> {
    public ParserStack() : base(200) { }
    public void Push(ParseTreeNode nodeInfo) {
      base.Add(nodeInfo);
    }
    public void Push(ParseTreeNode nodeInfo, ParserState state) {
      nodeInfo.State = state;
      base.Add(nodeInfo);
    }
    public ParseTreeNode Pop() {
      var top = Top;
      base.RemoveAt(Count - 1);
      return top;
    }
    public void Pop(int count) {
      base.RemoveRange(Count - count, count);
    }
    public ParseTreeNode Top {
      get {
        if (Count == 0) return null;
        return base[Count - 1];
      }
    }
  }
}
