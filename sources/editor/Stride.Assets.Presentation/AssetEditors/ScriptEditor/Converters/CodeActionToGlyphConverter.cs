// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2016, Eli Arbel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

using Microsoft.CodeAnalysis.CodeActions;

using RoslynPad.Roslyn.CodeActions;
using RoslynPad.Roslyn.Completion;

using Stride.Core.Presentation.ValueConverters;

namespace Stride.Assets.Presentation.AssetEditors.ScriptEditor.Converters
{
    internal sealed class CodeActionToGlyphConverter : OneWayValueConverter<CodeActionToGlyphConverter>
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var glyphNumber = ((CodeAction)value).GetGlyph();
            return Application.Current.TryFindResource(glyphNumber) as ImageSource;
        }
    }
}
