// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2011 Irony - Roman Ivantsov
// See the LICENSE.md file in the project root for full license information.

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



}//namespace
