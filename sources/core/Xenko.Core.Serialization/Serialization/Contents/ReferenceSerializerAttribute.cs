// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Core.Reflection;

namespace Xenko.Core.Serialization.Contents
{
    /// <summary>
    /// Used to detect whether a type is using <see cref="ReferenceSerializer"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    [AssemblyScan]
    public class ReferenceSerializerAttribute : Attribute
    {
    }
}
