// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2011 Irony - Roman Ivantsov
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Irony.Parsing.Construction {
  internal class LanguageDataBuilder { 

    internal LanguageData Language;
    Grammar _grammar;

    public LanguageDataBuilder(LanguageData language) {
      Language = language;
      _grammar = Language.Grammar;
    }

    public bool Build() {
      var sw = new Stopwatch(); 
      try {
        if (_grammar.Root == null)
          Language.Errors.AddAndThrow(GrammarErrorLevel.Error, null, Resources.ErrRootNotSet);
        sw.Start(); 
        var gbld = new GrammarDataBuilder(Language);
        gbld.Build();
        //Just in case grammar author wants to customize something...
        _grammar.OnGrammarDataConstructed(Language);
        var pbld = new ParserDataBuilder(Language);
        pbld.Build();
        Validate();
        //call grammar method, a chance to tweak the automaton
        _grammar.OnLanguageDataConstructed(Language);
        return true;
      } catch (GrammarErrorException) {
        return false; //grammar error should be already added to Language.Errors collection
      } finally {
        Language.ErrorLevel = Language.Errors.GetMaxLevel();
        sw.Stop(); 
        Language.ConstructionTime = sw.ElapsedMilliseconds; 
      }

    }

    #region Language Data Validation
    private void Validate() {

    }//method
    #endregion

  
  }//class
}
