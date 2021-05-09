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
