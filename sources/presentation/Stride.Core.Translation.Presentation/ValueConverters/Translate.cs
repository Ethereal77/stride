// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Globalization;

using Stride.Core.Annotations;

namespace Stride.Core.Translation.Presentation.ValueConverters
{
    public class Translate : LocalizableConverter<Translate>
    {
        public string Context { get; set; }

        /// <inheritdoc />
        [NotNull]
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = value?.ToString();
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            return string.IsNullOrEmpty(Context)
                ? TranslationManager.Instance.GetString(text, Assembly)
                : TranslationManager.Instance.GetParticularString(Context, text, Assembly);
        }
    }
}
