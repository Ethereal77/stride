// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Threading.Tasks;

namespace Stride.Core.Packages
{
    /// <summary>
    /// Generic interface for logging. See <see cref="MessageLevel"/> for various level of logging.
    /// </summary>
    public interface IPackagesLogger
    {
        /// <summary>
        /// Logs the <paramref name="message"/> using the log <paramref name="level"/>.
        /// </summary>
        /// <param name="level">The level of the logged message.</param>
        /// <param name="message">The message to log.</param>
        void Log(MessageLevel level, string message);

        /// <summary>
        /// Logs the <paramref name="message"/> using the log <paramref name="level"/>.
        /// </summary>
        /// <param name="level">The level of the logged message.</param>
        /// <param name="message">The message to log.</param>
        Task LogAsync(MessageLevel level, string message);
    }
}
