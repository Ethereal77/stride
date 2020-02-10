// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Core
{
    /// <summary>
    /// Interface ICollectorHolder for an instance that can collect other instance.
    /// </summary>
    public interface ICollectorHolder
    {
        /// <summary>
        /// Gets the collector.
        /// </summary>
        /// <value>The collector.</value>
        ObjectCollector Collector { get; }
    }
}
