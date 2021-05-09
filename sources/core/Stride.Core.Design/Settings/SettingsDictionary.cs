// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Stride.Core.Annotations;
using Stride.Core.Yaml.Events;

namespace Stride.Core.Settings
{
    /// <summary>
    /// An internal dictionary class used to serialize a <see cref="SettingsProfile"/>.
    /// </summary>
    [NonIdentifiableCollectionItems]
    internal class SettingsDictionary : Dictionary<string, List<ParsingEvent>>
    {
        // Used for temporary internal storage
        [DataMemberIgnore]
        internal SettingsProfile Profile;
    }
}
