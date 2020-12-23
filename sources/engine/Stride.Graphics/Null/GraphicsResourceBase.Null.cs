// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#if STRIDE_GRAPHICS_API_NULL

using System;

namespace Stride.Graphics
{
    public abstract partial class GraphicsResourceBase
    {
        /// <summary>
        ///   Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            NullHelper.ToImplement();
        }

        /// <summary>
        ///   Method called when the graphics device has been detected to be internally destroyed.
        /// </summary>
        protected internal virtual void OnDestroyed()
        {
            Destroyed?.Invoke(this, EventArgs.Empty);
            NullHelper.ToImplement();
        }

        /// <summary>
        ///   Method called when the graphics device has been recreated.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the resource has transitioned to a <see cref="GraphicsResourceLifetimeState.Active"/> state.
        /// </returns>
        protected internal virtual bool OnRecreate()
        {
            NullHelper.ToImplement();
            return false;
        }
    }
}
#endif
