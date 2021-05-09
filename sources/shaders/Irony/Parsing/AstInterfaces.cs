// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace Stride.Irony.Parsing {
  // These are generic interfaces for AST implementors. They define some basic interface that Parser needs to
  // construct AST tree. Note that we expect more than one interpreter/AST implementation: Irony.Interpreter.Ast
  // namespace provides just one of them. That's why these AST interfaces are here, and not in Interpreter.Ast namespace.
  // In the future, I plan to introduce advanced interpreter, with its own set of AST classes - it will probably live
  // in a separate assembly Irony.Interpreter2.dll.

  // Basic interface for AST nodes; Init method is the chance for AST node to get references to its child nodes, and all
  // related information gathered during parsing
  // Implementing this interface is a minimum required from custom AST node class to enable its creation by Irony
  // parser. Alternatively, if your custom AST node class does not implement this interface then you can create
  // and initialize node instances using AstNodeCreator delegate attached to corresponding non-terminal in your grammar.
  public interface IAstNodeInit {
    void Init(ParsingContext context, ParseTreeNode parseNode);
  }

  // Grammar explorer uses this interface to discover and display the AST tree after parsing the input
  // (Grammar Explorer additionally uses ToString method of the node to get the text representation of the node)
  public interface IBrowsableAstNode {
    SourceLocation Location { get; }
    IEnumerable GetChildNodes();
  }
}
