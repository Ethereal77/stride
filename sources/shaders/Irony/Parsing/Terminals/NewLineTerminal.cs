﻿// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2011 Irony - Roman Ivantsov
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Irony.Parsing {
  //This is a simple NewLine terminal recognizing line terminators for use in grammars for line-based languages like VB
  // instead of more complex alternative of using CodeOutlineFilter. 
  public class NewLineTerminal : Terminal {
    public NewLineTerminal(string name) : base(name, TokenCategory.Outline) {
      base.ErrorAlias = Resources.LabelLineBreak;  // "[line break]";
      this.Flags |= TermFlags.IsPunctuation;
    }

    public string LineTerminators = "\n\r\v";

    #region overrides: Init, GetFirsts, TryMatch
    public override void Init(GrammarData grammarData) {
      base.Init(grammarData);
      //Remove new line chars from whitespace
      foreach(char t in LineTerminators)
        grammarData.Grammar.WhitespaceChars = grammarData.Grammar.WhitespaceChars.Replace(t.ToString(), string.Empty);
    }
    public override IList<string> GetFirsts() {
      StringList firsts = new StringList();
      foreach(char t in LineTerminators)
        firsts.Add(t.ToString());
      return firsts;
    }
    public override Token TryMatch(ParsingContext context, ISourceStream source) {
      char current = source.PreviewChar;
      if (!LineTerminators.Contains(current)) return null;
      //Treat \r\n as a single terminator
      bool doExtraShift = (current == '\r' && source.NextPreviewChar == '\n');
      source.PreviewPosition++; //main shift
      if (doExtraShift)
        source.PreviewPosition++;
      Token result = source.CreateToken(this.OutputTerminal);
      return result;
    }

    #endregion

    
  }//class
}//namespace
