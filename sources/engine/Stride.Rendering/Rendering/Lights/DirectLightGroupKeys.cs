// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Rendering.Lights
{
    public static partial class DirectLightGroupKeys
    {
        public static ParameterKey<T> GetParameterKey<T>(ParameterKey<T> key, int lightGroupIndex)
        {
            if (key == null) throw new ArgumentNullException("key");
            return key.ComposeIndexer("directLightGroups", lightGroupIndex);
        }
    }
}
