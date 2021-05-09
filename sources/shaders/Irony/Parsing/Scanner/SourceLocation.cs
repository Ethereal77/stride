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

    public struct SourceLocation
    {
        public string SourceFilename;
        public int Position;
        public int Line;
        public int Column;
        public SourceLocation(int position, int line, int column, string sourceFilename = null)
        {
            SourceFilename = sourceFilename;
            Position = position;
            Line = line;
            Column = column;
        }
        //Line/col are zero-based internally
        public override string ToString()
        {
            return string.Format(Resources.FmtRowCol, SourceFilename == null ? "" : SourceFilename + " ", Line, Column);
        }

        public static int Compare(SourceLocation x, SourceLocation y)
        {
            if (x.Position < y.Position) return -1;
            if (x.Position == y.Position) return 0;
            return 1;
        }
    }//SourceLocation

  public struct SourceSpan {
    public readonly SourceLocation Location;
    public readonly int Length;
    public SourceSpan(SourceLocation location, int length) {
      Location = location;
      Length = length;
    }
    public int EndPosition {
      get { return Location.Position + Length; }
    }
  }


}//namespace
