// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2011 Irony - Roman Ivantsov
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Irony.Parsing {

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
    public void PopUntil(int finalCount) {
      if (finalCount < Count) 
        Pop(Count - finalCount); 
    }
    public ParseTreeNode Top {
      get {
        if (Count == 0) return null;
        return base[Count - 1];
      }
    }
  }
}
