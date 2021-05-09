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

  //Terminal report group is a facility for improving syntax error messages.
  // Irony parser/scanner reports an error like "Syntax error, invalid character. Expected: <expected list>."
  // The <expected list> is a list of all terminals (symbols) that are expected in current position.
  // This list might quite long and quite difficult to look through. The solution is to provide Group names for
  // groups of terminals - Group of type Normal.
  // Some terminals might be excluded from showing in expected list by including them into group of type Exclude.
  // Finally, Operator group allows you to specify group name for all operator symbols without listing operators -
  // Irony will collect all operator symbols registered with RegisterOperator method automatically.

  public enum TermReportGroupType {
    Normal,
    Exclude,
    Operator
  }
  public class TermReportGroup {
    public string Alias;
    public TermReportGroupType GroupType;
    public TerminalSet Terminals = new TerminalSet();
  }//class

  public class TermReportGroupList : List<TermReportGroup> { }

}//namespace
