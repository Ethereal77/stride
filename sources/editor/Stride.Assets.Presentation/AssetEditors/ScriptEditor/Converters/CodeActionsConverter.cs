// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2016, Eli Arbel
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

using Microsoft.CodeAnalysis.CodeActions;

using Stride.Roslyn.EditorServices.CodeActions;

using Stride.Core.Presentation.ValueConverters;

namespace Stride.Assets.Presentation.AssetEditors.ScriptEditor.Converters
{
    internal sealed class CodeActionsConverter : OneWayValueConverter<CodeActionsConverter>
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((CodeAction)value).GetCodeActions();
        }
    }
}
