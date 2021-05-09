// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Stride.Core.Presentation.Controls
{
    public static class TextBlockFormatting
    {
        public static Inline GetFormattedText(DependencyObject obj)
        {
            return (Inline)obj.GetValue(FormattedTextProperty);
        }

        public static void SetFormattedText(DependencyObject obj, Inline value)
        {
            obj.SetValue(FormattedTextProperty, value);
        }

        /// <summary>
        /// Identifies the dependency property which permits to directly bind a inline to a <see cref="TextBox"/>.
        /// </summary>
        public static readonly DependencyProperty FormattedTextProperty =
            DependencyProperty.RegisterAttached(
                "FormattedText",
                typeof(Inline),
                typeof(TextBlockFormatting),
                new PropertyMetadata(null, OnFormattedTextChanged));

        private static void OnFormattedTextChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var textBlock = o as TextBlock;
            if (textBlock == null) return;

            var inline = (Inline)e.NewValue;
            if (inline == null)
            {
                textBlock.Inlines.Clear();
            }
            else
            {
                textBlock.Inlines.Add(inline);
            }
        }
    }
}
