// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;

namespace Stride.Graphics
{
    /// <summary>
    /// Known values for <see cref="DepthStencilStateDescription"/>.
    /// </summary>
    public static class DepthStencilStates
    {
        /// <summary>
        /// A built-in state object with default settings for using a depth stencil buffer.
        /// </summary>
        public static readonly DepthStencilStateDescription Default = new DepthStencilStateDescription(true, true);

        /// <summary>
        /// A built-in state object with default settings using greater comparison for Z.
        /// </summary>
        public static readonly DepthStencilStateDescription DefaultInverse = new DepthStencilStateDescription(true, true) { DepthBufferFunction = CompareFunction.GreaterEqual };

        /// <summary>
        /// A built-in state object with settings for enabling a read-only depth stencil buffer.
        /// </summary>
        public static readonly DepthStencilStateDescription DepthRead = new DepthStencilStateDescription(true, false);

        /// <summary>
        /// A built-in state object with settings for not using a depth stencil buffer.
        /// </summary>
        public static readonly DepthStencilStateDescription None = new DepthStencilStateDescription(false, false);
    }
}
