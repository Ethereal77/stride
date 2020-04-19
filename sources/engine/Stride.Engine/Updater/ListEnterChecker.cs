// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;

namespace Xenko.Updater
{
    /// <summary>
    /// Implementation of <see cref="EnterChecker"/> for <see cref="IList{T}"/>.
    /// </summary>
    internal class ListEnterChecker<T> : EnterChecker
    {
        private readonly int minimumCount;

        public ListEnterChecker(int minimumCount)
        {
            this.minimumCount = minimumCount;
        }

        /// <inheritdoc/>
        public override bool CanEnter(object obj)
        {
            return minimumCount <= ((IList<T>)obj).Count;
        }
    }
}
