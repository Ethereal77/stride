// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2011 Irony - Roman Ivantsov
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Irony.Parsing {
  //Terminal based on custom method; allows creating custom match without creating new class derived from Terminal 
  public delegate Token MatchHandler(Terminal terminal, ParsingContext context, ISourceStream source);
  public class CustomTerminal : Terminal {
    public CustomTerminal(string name, MatchHandler handler, params string[] prefixes) : base(name) {
      _handler = handler;
      if (prefixes != null) 
        Prefixes.AddRange(prefixes);
      this.EditorInfo = new TokenEditorInfo(TokenType.Unknown, TokenColor.Text, TokenTriggers.None);
    }
    
    public readonly StringList Prefixes = new StringList();

    public MatchHandler Handler   {
      [System.Diagnostics.DebuggerStepThrough]
      get {return _handler;}
    } MatchHandler  _handler;

    public override Token TryMatch(ParsingContext context, ISourceStream source) {
      return _handler(this, context, source);
    }
    [System.Diagnostics.DebuggerStepThrough]
    public override IList<string> GetFirsts() {
      return Prefixes;
    }
  }//class


}
