// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2011 Irony - Roman Ivantsov
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Irony.Parsing {
  
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

    public TermReportGroup(string alias, TermReportGroupType groupType, IEnumerable<Terminal> terminals) {
      Alias = alias; 
      GroupType = groupType;
      if (terminals != null)
        Terminals.UnionWith(terminals); 
    }

  }//class

  public class TermReportGroupList : List<TermReportGroup> { }

}//namespace
