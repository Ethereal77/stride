// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core.Annotations;

namespace Stride.Core
{
    public static class StringSpanExtensions
    {
        /// <summary>
        /// Gets the substring with the specified span. If the span is invalid, return null.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="span">The span.</param>
        /// <returns>A substring with the specified span or null if span is empty.</returns>
        [CanBeNull]
        public static string Substring(this string str, StringSpan span)
        {
            return span.IsValid ? str.Substring(span.Start, span.Length) : null;
        }
    }
}
