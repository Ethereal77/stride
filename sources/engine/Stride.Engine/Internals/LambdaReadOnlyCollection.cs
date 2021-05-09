// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Stride.Internals
{
    internal class LambdaReadOnlyCollection<TSource, T> : IReadOnlyList<T>
    {
        private IReadOnlyList<TSource> source;
        private Func<TSource, T> selector;

        public LambdaReadOnlyCollection(IReadOnlyList<TSource> source, Func<TSource, T> selector)
        {
            this.source = source;
            this.selector = selector;
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            return source.Select(x => selector(x)).GetEnumerator();
        }

        /// <inheritdoc/>
        public int Count { get { return source.Count; } }

        /// <inheritdoc/>
        public T this[int index]
        {
            get { return selector(source[index]); }
        }
    }
}
