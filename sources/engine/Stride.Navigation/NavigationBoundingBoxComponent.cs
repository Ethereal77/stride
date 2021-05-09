// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Engine.Design;
using Stride.Navigation.Processors;

namespace Stride.Navigation
{
    /// <summary>
    /// A three dimensional bounding box  using the scale of the owning entity as the box extent. This is used to limit the area in which navigation meshes are generated
    /// </summary>
    [DataContract]
    [DefaultEntityComponentProcessor(typeof(BoundingBoxProcessor), ExecutionMode = ExecutionMode.All)]
    [Display("Navigation bounding box")]
    [ComponentCategory("Navigation")]
    public class NavigationBoundingBoxComponent : EntityComponent
    {
        /// <summary>
        /// The size of one edge of the bounding box
        /// </summary>
        [DataMember(0)]
        public Vector3 Size { get; set; } = Vector3.One;
    }
}
