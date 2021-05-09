// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.ObjectModel;

namespace Stride.Input
{
    /// <summary>
    /// A collection of <see cref="VirtualButtonConfig"/>.
    /// </summary>
    /// <remarks>
    /// Several virtual button configurations can be stored in this instance. 
    /// For example, User0 config could be stored on index 0, User1 on index 1...etc.
    /// </remarks>
    public class VirtualButtonConfigSet : Collection<VirtualButtonConfig>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualButtonConfigSet" /> class.
        /// </summary>
        public VirtualButtonConfigSet()
        {
        }

        public virtual float GetValue(InputManager inputManager, int configIndex, object name)
        {
            if (configIndex < 0 || configIndex >= Count)
            {
                return 0.0f;
            }

            var config = this[configIndex];
            return config != null ? config.GetValue(inputManager, name) : 0.0f;
        }
    }
}
