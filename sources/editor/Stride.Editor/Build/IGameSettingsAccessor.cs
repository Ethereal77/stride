// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Data;

namespace Stride.Editor.Build
{
    /// <summary>
    /// Used to access the game settings in a read-only way
    /// </summary>
    public interface IGameSettingsAccessor
    {
        /// <summary>
        /// Gets a copy of the requested <see cref="Configuration"/>. Can be null.
        /// </summary>
        /// <typeparam name="T">The requestted <see cref="Configuration"/></typeparam>
        /// <param name="profile">If not null, it will filter the results giving priority to the specified profile</param>
        /// <returns>The requested <see cref="Configuration"/> or null if not found</returns>
        T GetConfiguration<T>() where T : Configuration;
    }
}
