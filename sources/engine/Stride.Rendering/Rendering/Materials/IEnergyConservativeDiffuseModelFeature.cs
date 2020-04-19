// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Rendering.Materials
{
    /// <summary>
    /// Base interface for specular models supporting energy conservation.
    /// </summary>
    public interface IEnergyConservativeDiffuseModelFeature : IMaterialDiffuseModelFeature
    {
        /// <summary>
        /// A value indicating whether this instance is energy conservative.
        /// </summary>
        bool IsEnergyConservative { get; set; }
    }
}