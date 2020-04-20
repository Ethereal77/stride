// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core;

namespace Stride.Rendering
{
    /// <summary>
    /// Defines how a <see cref="RenderObject"/> gets assigned to specific <see cref="RenderStage"/>.
    /// </summary>
    [DataContract(Inherited = true)]
    public abstract class RenderStageSelector
    {
        public abstract void Process(RenderObject renderObject);
    }
}
