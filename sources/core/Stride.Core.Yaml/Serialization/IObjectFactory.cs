// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.Yaml.Serialization
{
    /// <summary>
    ///   Provides a method to create an object instance of a certain type.
    /// </summary>
    /// <remarks>
    ///   This interface allows to provide custom logic for creating instances during deserialization.
    /// </remarks>
    public interface IObjectFactory
    {
        /// <summary>
        ///   Creates an instance of the specified type.
        /// </summary>
        /// <returns>Instance of the specified <paramref name="type"/>; or <c>null</c> if it couldn't be created.</returns>
        object Create(Type type);
    }
}
