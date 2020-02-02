﻿// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2011 Irony - Roman Ivantsov
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Irony.Parsing {

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
        //Line and Column displayed to user should be 1-based
        public string ToUiString()
        {
            return string.Format(Resources.FmtRowCol, SourceFilename == null ? "" : SourceFilename + " ", Line + 1, Column + 1);
        }
        public static int Compare(SourceLocation x, SourceLocation y)
        {
            if (x.Position < y.Position) return -1;
            if (x.Position == y.Position) return 0;
            return 1;
        }
        public static SourceLocation Empty
        {
            get { return _empty; }
        } static SourceLocation _empty = new SourceLocation();

        public static SourceLocation operator +(SourceLocation x, SourceLocation y)
        {
            return new SourceLocation(x.Position + y.Position, x.Line + y.Line, x.Column + y.Column);
        }
        public static SourceLocation operator +(SourceLocation x, int offset)
        {
            return new SourceLocation(x.Position + offset, x.Line, x.Column + offset);
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
