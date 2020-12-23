// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Stride.Core;
using Stride.Core.Serialization;
using Stride.Core.Serialization.Contents;
using Stride.Graphics;
using Stride.Rendering.Materials;

namespace Stride.Rendering
{
    /// <summary>
    ///   Represents a rendering material, a compiled version of <see cref="MaterialDescriptor"/>.
    /// </summary>
    [ReferenceSerializer, DataSerializerGlobal(typeof(ReferenceSerializer<Material>), Profile = "Content")]
    [ContentSerializer(typeof(DataContentSerializer<Material>))]
    [DataContract]
    public class Material
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="Material"/> class.
        /// </summary>
        public Material()
        {
            Passes = new MaterialPassCollection(this);
        }

        /// <summary>
        ///   The passes contained in this material (usually one).
        /// </summary>
        public MaterialPassCollection Passes { get; }

        /// <summary>
        ///   Gets or sets the descriptor.
        /// </summary>
        /// <value>The descriptor of the material. This property is <c>null</c> at runtime.</value>
        [DataMemberIgnore]
        public MaterialDescriptor Descriptor { get; set; }

        /// <summary>
        ///   Creates a new material from the specified descriptor.
        /// </summary>
        /// <param name="device">The graphics device.</param>
        /// <param name="descriptor">The material descriptor.</param>
        /// <returns>An instance of a <see cref="Material"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="descriptor"/> is a <c>null</c> reference.</exception>
        /// <exception cref="InvalidOperationException">An error has occured with the material description.</exception>
        public static Material New(GraphicsDevice device, MaterialDescriptor descriptor)
        {
            if (descriptor is null)
                throw new ArgumentNullException(nameof(descriptor));

            // The descriptor is not assigned to the material because:
            //  1) We don't know whether it will mutate and be used to generate another material.
            //  2) We don't want to hold on to memory we actually don't need.
            var context = new MaterialGeneratorContext(new Material(), device)
            {
                GraphicsProfile = device.Features.RequestedProfile,
            };
            var result = MaterialGenerator.Generate(descriptor, context, $"{descriptor.MaterialId}:RuntimeMaterial");

            if (result.HasErrors)
                throw new InvalidOperationException($"Error when creating the material [{result.ToText()}].");

            return result.Material;
        }
    }
}
