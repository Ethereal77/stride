// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Stride.Core.Yaml.Events;

namespace Stride.Core.Yaml
{
    /// <summary>
    /// Objects that can't be loaded as valid Yaml will be converted to a proxy object implementing this interface by <see cref="ErrorRecoverySerializer"/>.
    /// </summary>
    public interface IUnloadable
    {
        string TypeName { get; }

        string AssemblyName { get; }

        string Error { get; }

        List<ParsingEvent> ParsingEvents { get; }
    }
}
