// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

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
