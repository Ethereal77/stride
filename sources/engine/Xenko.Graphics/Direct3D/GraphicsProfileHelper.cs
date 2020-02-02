// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#if XENKO_GRAPHICS_API_DIRECT3D 

using SharpDX.Direct3D;

namespace Xenko.Graphics
{
    internal static class GraphicsProfileHelper
    {
        /// <summary>
        /// Returns a GraphicsProfile from a FeatureLevel.
        /// </summary>
        /// <returns>associated GraphicsProfile</returns>
        public static FeatureLevel[] ToFeatureLevel(this GraphicsProfile[] profiles)
        {
            if (profiles == null)
            {
                return null;
            }

            var levels = new FeatureLevel[profiles.Length];
            for (int i = 0; i < levels.Length; i++)
            {
                levels[i] = (FeatureLevel)profiles[i];
            }
            return levels;
        }
        
        /// <summary>
        /// Returns a GraphicsProfile from a FeatureLevel.
        /// </summary>
        /// <returns>associated GraphicsProfile</returns>
        public static FeatureLevel ToFeatureLevel(this GraphicsProfile profile)
        {
            return (FeatureLevel)profile;
        }

        /// <summary>
        /// Returns a GraphicsProfile from a FeatureLevel.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <returns>associated GraphicsProfile</returns>
        public static GraphicsProfile FromFeatureLevel(FeatureLevel level)
        {
            return (GraphicsProfile)level;
        }
    }
} 
#endif 
