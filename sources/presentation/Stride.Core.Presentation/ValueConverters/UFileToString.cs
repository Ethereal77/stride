// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Globalization;
using System.IO;

using Stride.Core.IO;

namespace Stride.Core.Presentation.ValueConverters
{
    /// <summary>
    /// This converter will convert an <see cref="UFile"/> object to its string representation. <see cref="ConvertBack"/> is supported.
    /// </summary>
    /// <seealso cref="UDirectoryToString"/>
    public class UFileToString : ValueConverterBase<UFileToString>
    {
        /// <inheritdoc/>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString().Replace('/', Path.DirectorySeparatorChar);
        }

        /// <inheritdoc/>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            try
            {
                return new UFile((string)value);
            }
            catch
            {
                return new UFile("");
            }
        }
    }
}
