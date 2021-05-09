// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;

namespace Stride.Rendering.Materials.ComputeColors
{
    /// <summary>
    ///   Represents a shader that computes a color / vector value.
    /// </summary>
    [DataContract("ComputeShaderClassColor")]
    [Display("Shader")]
    public class ComputeShaderClassColor : ComputeShaderClassBase<IComputeColor>, IComputeColor
    {
        private int hashCode = 0;

        /// <inheritdoc/>
        public bool HasChanged
        {
            get
            {
                var mixinHash = MixinReference?.GetHashCode() ?? 0;

                if (hashCode != 0 && hashCode == mixinHash)
                    return false;

                hashCode = mixinHash;
                return true;
            }
        }
    }
}
