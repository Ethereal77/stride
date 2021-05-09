// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Shaders.Ast;
using Stride.Core.Shaders.Visitor;

namespace Stride.Shaders.Parser.Mixins
{
    /// <summary>
    /// Class to replace a node by another in an AST
    /// </summary>
    internal class StrideReplaceVisitor : ShaderRewriter
    {
        #region Private members

        /// <summary>
        /// The node to replace
        /// </summary>
        protected Node nodeToReplace;

        /// <summary>
        /// the replacement node
        /// </summary>
        protected Node replacementNode;

        /// <summary>
        /// a boolean stating that the operation is complete
        /// </summary>
        protected bool complete = false;

        #endregion

        #region Constructor

        public StrideReplaceVisitor(Node toReplace, Node replacement) : base(false, false)
        {
            nodeToReplace = toReplace;
            replacementNode = replacement;
        }

        #endregion

        #region Public method

        public bool Run(Node startNode)
        {
            VisitDynamic(startNode);

            return complete;
        }

        #endregion

        #region Protected method

        public override Node DefaultVisit(Node node)
        {
            if (node == nodeToReplace)
            {
                complete = true;
                return replacementNode;
            }
            
            return base.DefaultVisit(node);
        }

        #endregion
    }
}
