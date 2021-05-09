// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Threading;

using Stride.Graphics;

namespace Stride.Rendering
{
    public struct ResourceGroupEntry
    {
        public int LastFrameUsed;
        public ResourceGroup Resources;

        /// <summary>
        /// Mark resource group as used during this frame.
        /// </summary>
        /// <returns>True if state changed (object was not mark as used during this frame until now), otherwise false.</returns>
        public bool MarkAsUsed(RenderSystem renderSystem)
        {
            return Interlocked.Exchange(ref LastFrameUsed, renderSystem.FrameCounter) != renderSystem.FrameCounter;
        }
    }
}
