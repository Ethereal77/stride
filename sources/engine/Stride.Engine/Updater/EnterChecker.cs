// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Updater
{
    /// <summary>
    /// Provides a way to perform additional checks when entering an object (typically out of bounds checks).
    /// </summary>
    public abstract class EnterChecker
    {
        /// <summary>
        /// Called by <see cref="UpdateEngine.Run"/> to perform additional checks when entering an object (typically out of bounds checks).
        /// </summary>
        /// <param name="obj">The object being entered.</param>
        /// <returns>True if checks succeed, false otherwise.</returns>
        public abstract bool CanEnter(object obj);
    }
}
