﻿// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2011 Irony - Roman Ivantsov
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Irony.Parsing;
using Irony.Parsing.Construction;

namespace Irony.Parsing {
  public static class ParserDataPrinter { 

    public static string PrintStateList(LanguageData language) {
      StringBuilder sb = new StringBuilder();
      foreach (ParserState state in language.ParserData.States) {
        sb.Append("State " + state.Name); 
        if (state.BuilderData.IsInadequate) sb.Append(" (Inadequate)");
        sb.AppendLine();
        var srConflicts = state.BuilderData.GetShiftReduceConflicts();
        if (srConflicts.Count > 0)
          sb.AppendLine("  Shift-reduce conflicts on inputs: " + srConflicts.ToString());
        var ssConflicts = state.BuilderData.GetReduceReduceConflicts();
        if (ssConflicts.Count > 0)
          sb.AppendLine("  Reduce-reduce conflicts on inputs: " + ssConflicts.ToString());
        //LRItems
        if (state.BuilderData.ShiftItems.Count > 0) {
          sb.AppendLine("  Shift items:");
          foreach (var item in state.BuilderData.ShiftItems)
            sb.AppendLine("    " + item.ToString());
        }
        if (state.BuilderData.ReduceItems.Count > 0) {
          sb.AppendLine("  Reduce items:");
          foreach(LRItem item in state.BuilderData.ReduceItems) {
            var sItem = item.ToString();
            if (item.Lookaheads.Count > 0)
              sItem += " [" + item.Lookaheads.ToString() + "]";
            sb.AppendLine("    " + sItem);
          }
        }
        sb.Append("  Transitions: ");
        bool atFirst = true; 
        foreach (BnfTerm key in state.Actions.Keys) {
          ParserAction action = state.Actions[key];
          var hasTransitions = action.ActionType == ParserActionType.Shift || action.ActionType == ParserActionType.Operator;
          if (!hasTransitions)
            continue;
          if (!atFirst) sb.Append(", ");
          atFirst = false; 
          sb.Append(key.ToString());
          sb.Append("->");
          sb.Append(action.NewState.Name);
        }
        sb.AppendLine();
        sb.AppendLine();
      }//foreach
      return sb.ToString();
    }

    public static string PrintTerminals(LanguageData language) {
      StringBuilder sb = new StringBuilder();
      foreach (Terminal term in language.GrammarData.Terminals) {
        sb.Append(term.ToString());
        sb.AppendLine();
      }
      return sb.ToString();
    }

    public static string PrintNonTerminals(LanguageData language) {
      StringBuilder sb = new StringBuilder();
      foreach (NonTerminal nt in language.GrammarData.NonTerminals) {
        sb.Append(nt.Name);
        sb.Append(nt.FlagIsSet(TermFlags.IsNullable) ? "  (Nullable) " : string.Empty);
        sb.AppendLine();
        foreach (Production pr in nt.Productions) {
          sb.Append("   ");
          sb.AppendLine(pr.ToString());
        }
      }//foreachc nt
      return sb.ToString();
    }

  }//class
}//namespace
