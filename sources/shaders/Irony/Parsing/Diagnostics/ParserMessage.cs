// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2011 Irony - Roman Ivantsov
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Irony.Parsing {

  public enum ParserErrorLevel {
    Info = 0,
    Warning = 1,
    Error = 2,
  }

  //Container for syntax errors and warnings
  public class ParserMessage {
    public ParserMessage(ParserErrorLevel level, SourceLocation location, string message, ParserState parserState) {
      Level = level; 
      Location = location;
      Message = message;
      ParserState = parserState;
    }

    public readonly ParserErrorLevel Level;
    public readonly ParserState ParserState;
    public readonly SourceLocation Location;
    public readonly string Message;

    public override string ToString() {
      return Message;
    }
  }//class

  public class ParserMessageList : List<ParserMessage> {
    public static int ByLocation(ParserMessage x, ParserMessage y) {
      return SourceLocation.Compare(x.Location, y.Location);
    }
  }

}//namespace
