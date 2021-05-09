// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Engine;

namespace Stride.Editor.Preview
{
    public class PreviewEntity
    {
        /// <summary>
        /// The entity to preview.
        /// </summary>
        public Entity Entity;

        /// <summary>
        /// The actions to undertake when the preview entity is not used anymore.
        /// </summary>
        public Action Disposed;

        public PreviewEntity(Entity entity)
        {
            Entity = entity;
        }
    }
}
