// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2011 Irony - Roman Ivantsov
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

using Irony.Parsing;
using Irony.Interpreter.Ast;

namespace Irony.Interpreter { 

  public class StackFrame {
    public readonly EvaluationContext Context;
    public string MethodName; //for debugging purposes
    public StackFrame Parent; //Lexical parent - not the same as the caller
    public StackFrame Caller;
    internal ValuesTable Values; //global values for top frame; parameters and local variables for method frame

    public StackFrame(EvaluationContext context, ValuesTable globals) {
      Context = context; 
      Values = globals;
      if (Values == null)
        Values = new ValuesTable(100);
    }

    public StackFrame(EvaluationContext context, string methodName, StackFrame caller, StackFrame parent) {
      MethodName = methodName; 
      Caller = caller;
      Parent = parent;
      Values = new ValuesTable(8); 
    }

  }//class

}//namespace
