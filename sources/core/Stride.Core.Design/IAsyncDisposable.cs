// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Threading.Tasks;

namespace Stride.Core
{
    /// <summary>
    ///   Provides a mechanism to free unmanaged resources asynchronously.
    /// </summary>
    public interface IAsyncDisposable
    {
        /// <summary>
        ///   Does asynchronously application-specific tasks to free or reset unmanaged resources.
        /// </summary>
        /// <returns>A <see cref="Task"/> that completes when this instance has been disposed.</returns>
        Task DisposeAsync();
    }
}
