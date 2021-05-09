// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2011 Irony - Roman Ivantsov
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Stride.Irony.Parsing {

  public class StringSet : HashSet<string> {
    public StringSet() { }
    public StringSet(StringComparer comparer) : base(comparer) { }
    public override string ToString() {
      return ToString(" ");
    }
    public string ToString(string separator) {
      return string.Join(separator, this);
    }
  }
}
