// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2011 Irony - Roman Ivantsov
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Irony.Parsing { 

  //Container for syntax error
  public class SyntaxError {
    public SyntaxError(SourceLocation location, string message, ParserState parserState) {
      Location = location;
      Message = message;
      ParserState = parserState;
    }

    public readonly SourceLocation Location;
    public readonly string Message;
    public ParserState ParserState; 

    public override string ToString() {
      return Message;
    }
  }//class

  public class SyntaxErrorList : List<SyntaxError> {
    public static int ByLocation(SyntaxError x, SyntaxError y) {
      return SourceLocation.Compare(x.Location, y.Location);
    }
  }

}//namespace
