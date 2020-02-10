// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Irony.Parsing {
  public class ParsingEventArgs : EventArgs {
    public readonly ParsingContext Context;
    public ParsingEventArgs(ParsingContext context) {
      Context = context;
    }
  }
}
