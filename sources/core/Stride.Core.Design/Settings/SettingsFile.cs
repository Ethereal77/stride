// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.Settings
{
    /// <summary>
    /// An internal class that represents a set of settings that can be stored in a file.
    /// </summary>
    [DataContract("SettingsFile")]
    internal class SettingsFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsFile"/> class.
        /// </summary>
        public SettingsFile(SettingsProfile profile)
        {
            Settings = profile;
        }

        /// <summary>
        /// Gets the settings profile to serialize.
        /// </summary>
        [DataMemberCustomSerializer]
        public SettingsProfile Settings { get; private set; }
    }
}
