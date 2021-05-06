// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;

using Stride.Core;

namespace Stride.Rendering.ProceduralModels
{
    /// <summary>
    ///   Defines the methods to create a procedural model.
    /// </summary>
    public interface IProceduralModel
    {
        /// <summary>
        ///   Creates the procedural model.
        /// </summary>
        /// <param name="services">The services registry.</param>
        /// <param name="model">A model instance to fill with the procedural content.</param>
        void Generate(IServiceRegistry services, Model model);

        /// <summary>
        ///   Sets the material for the procedural model
        /// </summary>
        /// <param name="name">Name of the material slot.</param>
        /// <param name="material">The material.</param>
        void SetMaterial(string name, Material material);

        /// <summary>
        ///   Gets the collection of material instances used by this <see cref="IProceduralModel"/>/
        /// </summary>
        [Display(Browsable = false)]
        IEnumerable<KeyValuePair<string, MaterialInstance>> MaterialInstances { get; }
    }
}
