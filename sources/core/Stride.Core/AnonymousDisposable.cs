// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Core.Annotations;

namespace Xenko.Core
{
    /// <summary>
    /// This class allows implementation of <see cref="IDisposable"/> using anonymous functions.
    /// The anonymous function will be invoked only on the first call to the <see cref="Dispose"/> method.
    /// </summary>
    public sealed class AnonymousDisposable : IDisposable
    {
        private bool isDisposed;
        private Action onDispose;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnonymousDisposable"/> class.
        /// </summary>
        /// <param name="onDispose">The anonymous function to invoke when this object is disposed.</param>
        public AnonymousDisposable([NotNull] Action onDispose)
        {
            if (onDispose == null) throw new ArgumentNullException(nameof(onDispose));

            this.onDispose = onDispose;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (isDisposed)
                return;

            isDisposed = true;

            onDispose();
            onDispose = null;
        }
    }
}
