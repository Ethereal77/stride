// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Windows;
using System.Windows.Markup;

using Stride.Core.Presentation.Internal;

namespace Stride.Core.Presentation.MarkupExtensions
{
    [MarkupExtensionReturnType(typeof(Visibility))]
    public class CollapsedExtension : MarkupExtension
    {
        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return VisibilityBoxes.CollapsedBox;
        }
    }

    [MarkupExtensionReturnType(typeof(Visibility))]
    public class HiddenExtension : MarkupExtension
    {
        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return VisibilityBoxes.HiddenBox;
        }
    }

    [MarkupExtensionReturnType(typeof(Visibility))]
    public class VisibleExtension : MarkupExtension
    {
        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return VisibilityBoxes.VisibleBox;
        }
    }
}
