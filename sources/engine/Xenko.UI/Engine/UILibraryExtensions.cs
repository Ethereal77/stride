// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Engine.Design;
using Xenko.UI;

namespace Xenko.Engine
{
    public static class UILibraryExtensions
    {
        /// <summary>
        /// Instantiates a copy of the element of the library identified by <paramref name="name"/>.
        /// </summary>
        /// <typeparam name="TElement">The type of the element.</typeparam>
        /// <param name="library">The library.</param>
        /// <param name="name">The name of the element in the library.</param>
        /// <returns></returns>
        public static TElement InstantiateElement<TElement>(this UILibrary library, string name)
            where TElement : UIElement
        {
            if (library == null) throw new ArgumentNullException(nameof(library));

            UIElement source;
            if (library.UIElements.TryGetValue(name, out source))
            {
                return UICloner.Clone(source) as TElement;
            }
            return null;
        }
    }
}
