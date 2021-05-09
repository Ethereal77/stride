// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Yaml
{
    /// <summary>
    /// Placeholder value to remove keys from <see cref="DynamicYamlMapping"/>.
    /// </summary>
    public class DynamicYamlEmpty : DynamicYamlObject
    {
        public static readonly DynamicYamlEmpty Default = new DynamicYamlEmpty();    
    }
}
