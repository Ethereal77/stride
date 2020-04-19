// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Windows;
using System.Windows.Markup;

using Xenko.Core.Annotations;

namespace Xenko.Core.Presentation.MarkupExtensions
{
    [MarkupExtensionReturnType(typeof(SizeExtension))]
    public class SizeExtension : MarkupExtension
    {
        public SizeExtension(double uniformLength)
        {
            Value = new Size(uniformLength, uniformLength);
        }

        public SizeExtension(double width, double height)
        {
            Value = new Size(width, height);
        }

        public SizeExtension(Size value)
        {
            Value = value;
        }

        public Size Value { get; set; }

        [NotNull]
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Value;
        }
    }
}
