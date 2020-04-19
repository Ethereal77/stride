// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Windows;
using System.Windows.Markup;

using Xenko.Core.Annotations;

namespace Xenko.Core.Presentation.MarkupExtensions
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
