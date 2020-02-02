﻿// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Irony.Parsing;
using Irony.Interpreter;

namespace Irony.Interpreter.Ast {
  //A stub to use when AST node was not created (type not specified on NonTerminal, or error on creation)
  // The purpose of the stub is to throw a meaningful message when interpreter tries to evaluate null node.
  public class NullNode : AstNode {

    public NullNode(BnfTerm term) {
      this.Term = term; 
    }
        
    public override void Evaluate(EvaluationContext context, AstMode mode) {
      context.ThrowError(Resources.ErrNullNodeEval, this.Term);  
    }
  }//class
}
