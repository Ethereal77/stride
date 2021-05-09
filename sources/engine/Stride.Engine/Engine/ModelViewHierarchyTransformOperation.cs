// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Engine
{
    /// <summary>
    /// Updates <see cref="Engine.ModelComponent.Skeleton"/>.
    /// </summary>
    public class ModelViewHierarchyTransformOperation : TransformOperation
    {
        public readonly ModelComponent ModelComponent;

        public ModelViewHierarchyTransformOperation(ModelComponent modelComponent)
        {
            ModelComponent = modelComponent;
        }

        /// <inheritdoc/>
        public override void Process(TransformComponent transformComponent)
        {
            ModelComponent.Update(transformComponent);
        }
    }
}
