// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Globalization;

using Xenko.Core.Assets.Editor.ViewModel;
using Xenko.Core.Presentation.ValueConverters;
using Xenko.Editor.Build;

namespace Xenko.Assets.Presentation.ValueConverters
{
    /// <summary>
    /// This value converter will convert any numeric value to integer. <see cref="ConvertBack"/> is supported and
    /// will convert the value to the target if it is numeric, otherwise it returns the value as-is.
    /// </summary>
    public class TimeToFrames : ValueConverterBase<TimeToFrames>
    {
        /// <inheritdoc/>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            var gameSettings = SessionViewModel.Instance?.ServiceProvider.Get<GameSettingsProviderService>().CurrentGameSettings;
            var frameRate = (double) (gameSettings?.GetOrCreate<EditorSettings>().AnimationFrameRate ?? 30);
            frameRate = Math.Max(frameRate, 1.0);
            var timeSpan = (TimeSpan)value;
            return System.Convert.ChangeType(timeSpan.TotalSeconds * frameRate + 0.1, typeof(long));
        }

        /// <inheritdoc/>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var gameSettings = SessionViewModel.Instance?.ServiceProvider.Get<GameSettingsProviderService>().CurrentGameSettings;

            var frameRate = (double)(gameSettings?.GetOrCreate<EditorSettings>().AnimationFrameRate ?? 30);

            frameRate = Math.Max(frameRate, 1.0);

            var scalar = (double)(value ?? default(double));
            return TimeSpan.FromSeconds(scalar / frameRate);
        }
    }
}

