// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Shaders.Ast;
using Xenko.Core.Shaders.Visitor;

namespace Xenko.Shaders.Parser.Mixins
{
    /// <summary>
    /// Class to replace a node by another in an AST
    /// </summary>
    internal class XenkoReplaceVisitor : ShaderRewriter
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

        public XenkoReplaceVisitor(Node toReplace, Node replacement) : base(false, false)
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
