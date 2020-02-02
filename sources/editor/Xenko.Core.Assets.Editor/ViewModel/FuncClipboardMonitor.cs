// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Core;
using Xenko.Core.Annotations;
using Xenko.Core.Presentation.Interop;

namespace Xenko.Core.Assets.Editor.ViewModel
{
    public class FuncClipboardMonitor<TResult> : IDestroyable
    {
        private TResult cachedResult;
        private bool isValid;

        public FuncClipboardMonitor()
        {
            ClipboardMonitor.ClipboardTextChanged += OnClipboardTextChanged;
        }

        /// <summary>
        /// If the clipboard has changed since the previous call or the value has been invalidated, evaluates the given <paramref name="func"/> then caches and returns its result.
        /// Otherwise, returns the previously cached result.
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public TResult Get([NotNull] Func<TResult> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));

            if (!isValid)
            {
                isValid = true;
                cachedResult = func();
            }

            return cachedResult;
        }

        /// <summary>
        /// Invalidates the cached value.
        /// </summary>
        /// <remarks>The next time <see cref="Get"/> is called, the function will be evaluated regardless of whether the clipboard had changed or not.</remarks>
        public void Invalidate()
        {
            isValid = false;
        }

        private void OnClipboardTextChanged(object sender, EventArgs eventArgs)
        {
            Invalidate();
        }

        /// <inheritdoc />
        public void Destroy()
        {
            ClipboardMonitor.ClipboardTextChanged -= OnClipboardTextChanged;
        }
    }
}
