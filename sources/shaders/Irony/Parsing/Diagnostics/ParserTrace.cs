// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2011 Irony - Roman Ivantsov
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stride.Irony.Parsing {
  public class ParserTraceEntry {
    public ParserState State;
    public ParseTreeNode StackTop;
    public ParseTreeNode Input;
    public string Message;
    public bool IsError;

    public ParserTraceEntry(ParserState state, ParseTreeNode stackTop, ParseTreeNode input, string message, bool isError) {
      State = state;
      StackTop = stackTop;
      Input = input;
      Message = message;
      IsError = isError;
    }
  }//class

  public class ParserTrace : List<ParserTraceEntry> { }

  public class ParserTraceEventArgs : EventArgs {
    public ParserTraceEventArgs(ParserTraceEntry entry) {
      Entry = entry; 
    }

    public readonly ParserTraceEntry Entry;

    public override string ToString() {
      return Entry.ToString(); 
    }
  }//class



}//namespace
