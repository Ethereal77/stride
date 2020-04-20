// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core.Mathematics;
using Stride.Engine;

namespace Stride.Assets.Presentation.AssetEditors.GameEditor
{
    public struct TransformationTRS
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;

        public TransformationTRS(TransformComponent transform)
        {
            Position = transform.Position;
            Rotation = transform.Rotation;
            Scale = transform.Scale;
        }
    }
}
