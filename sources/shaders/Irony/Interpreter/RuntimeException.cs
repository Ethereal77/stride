// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2011 Irony - Roman Ivantsov
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Irony.Parsing;

namespace Irony.Interpreter { 
  public class RuntimeException : Exception {
    public SourceLocation Location;
    public RuntimeException(string message) : base(message) {   }
    public RuntimeException(string message, Exception inner) : base(message, inner) {   }
    public RuntimeException(string message, Exception inner, SourceLocation location) : base(message, inner) {
      Location = location;
    }

  }
}
