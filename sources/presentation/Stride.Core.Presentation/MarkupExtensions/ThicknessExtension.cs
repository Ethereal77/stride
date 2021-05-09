// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Markup;

using Stride.Core.Annotations;

namespace Stride.Core.Presentation.MarkupExtensions
{
    [MarkupExtensionReturnType(typeof(ThicknessExtension))]
    public class ThicknessExtension : MarkupExtension
    {
        public ThicknessExtension(double uniformLength)
        {
            Value = new Thickness(uniformLength);
        }

        public ThicknessExtension(double horizontal, double vertical)
        {
            Value = new Thickness(horizontal, vertical, horizontal, vertical);
        }

        public ThicknessExtension(double left, double top, double right, double bottom)
        {
            Value = new Thickness(left, top, right, bottom);
        }

        public ThicknessExtension(Thickness value)
        {
            Value = value;
        }

        public Thickness Value { get; set; }

        [NotNull]
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Value;
        }
    }
}
