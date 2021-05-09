// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Rendering
{
    /// <summary>
    /// Assembly attribute used to mark assembly that has been preprocessed using the <see cref="ParameterKeyProcessor"/>.
    /// Assemblies without this attribute will have all of their type members tagged with <see cref="EffectKeysAttribute"/> scanned for <see cref="ParameterKey"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class AssemblyEffectKeysAttribute : Attribute
    {
    }
}
