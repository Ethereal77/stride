// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Rendering.Lights
{
    public static class EnvironmentLightKeys
    {
        public static ParameterKey<T> GetParameterKey<T>(ParameterKey<T> key, int lightIndex)
        {
            if (key == null) throw new ArgumentNullException("key");
            return key.ComposeIndexer("environmentLights", lightIndex);
        }
    }
}
