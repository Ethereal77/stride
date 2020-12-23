// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core;
using Stride.Graphics;

namespace Stride.Rendering
{
    /// <summary>
    ///   Represents a single rendering pass of a <see cref="Rendering.Material"/>.
    /// </summary>
    [DataContract]
    public class MaterialPass
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="MaterialPass"/> class.
        /// </summary>
        public MaterialPass()
        {
            Parameters = new ParameterCollection();
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="MaterialPass"/> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public MaterialPass(ParameterCollection parameters)
        {
            Parameters = parameters;
        }


        /// <summary>
        ///   Gets the material that contains this pass.
        /// </summary>
        [DataMemberIgnore]
        public Material Material { get; internal set; }

        /// <summary>
        ///   Gets or sets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public ParameterCollection Parameters { get; set; }

        /// <summary>
        ///   The culling mode to use overriding the one defined by the material.
        /// </summary>
        public CullMode? CullMode;

        /// <summary>
        ///   The blending state to use overriding the one defined by the material.
        /// </summary>
        public BlendStateDescription? BlendState;

        /// <summary>
        ///   The tessellation method used by the material.
        /// </summary>
        public StrideTessellationMethod TessellationMethod;

        /// <summary>
        ///   Gets or sets a value indicating whether this material pass has transparency.
        /// </summary>
        /// <value><c>true</c> if this instance has transparency; otherwise, <c>false</c>.</value>
        public bool HasTransparency { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to use the alpha-to-coverage multisampling technique.
        /// </summary>
        public bool? AlphaToCoverage { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating if this material is affected by lighting.
        /// </summary>
        /// <value><c>true</c> if this instance is affected by lighting; otherwise, <c>false</c>.</value>
        public bool IsLightDependent { get; set; }

        /// <summary>
        ///   Gets or sets the index of this pass as part of its containing <see cref="Material"/>.
        /// </summary>
        /// <remarks>
        ///   The index is used for state sorting.
        /// </remarks>
        public int PassIndex { get; set; }
    }
}
