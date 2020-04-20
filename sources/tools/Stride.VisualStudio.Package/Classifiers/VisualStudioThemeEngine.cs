// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2014 F# Software Foundation
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Microsoft.VisualStudio.PlatformUI;

namespace Stride.VisualStudio.Classifiers
{
    internal class VisualStudioThemeEngine : IDisposable
    {
        private dynamic colorThemeService;

        private readonly Dictionary<Guid, VisualStudioTheme> availableThemes = new Dictionary<Guid, VisualStudioTheme>
        {
            { new Guid("DE3DBBCD-F642-433C-8353-8F1DF4370ABA"), VisualStudioTheme.Light },
            { new Guid("A4D6A176-B948-4B29-8C66-53C97A1ED7D0"), VisualStudioTheme.Blue },
            { new Guid("1DED0138-47CE-435E-84EF-9EC1F439B749"), VisualStudioTheme.Dark }
        };

        public event EventHandler OnThemeChanged;

        public VisualStudioThemeEngine(IServiceProvider serviceProvider)
        {
            colorThemeService = serviceProvider.GetService(typeof(SVsColorThemeService));

            VSColorTheme.ThemeChanged += RaiseThemeChanged;
        }

        public void Dispose()
        {
            VSColorTheme.ThemeChanged -= RaiseThemeChanged;
        }

        public VisualStudioTheme GetCurrentTheme()
        {
            if (colorThemeService == null)
            {
                return GuessUnknownTheme();
            }

            var themeGuid = (Guid)colorThemeService.CurrentTheme.ThemeId;


            VisualStudioTheme theme;
            if (!availableThemes.TryGetValue(themeGuid, out theme))
            {
                return GuessUnknownTheme();
            }

            return theme;
        }

        private VisualStudioTheme GuessUnknownTheme()
        {
            var backgroundColor = VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowBackgroundColorKey);
            return (backgroundColor.R > 0x80) ? VisualStudioTheme.UnknownLight : VisualStudioTheme.UnknownDark;
        }

        private void RaiseThemeChanged(ThemeChangedEventArgs e)
        {
            OnThemeChanged?.Invoke(this, EventArgs.Empty);
        }

        // Source: http://stackoverflow.com/q/29943319 (note: didn't work to cast it as IVsColorThemeService so using dynamic)
        [ComImport]
        [Guid("0D915B59-2ED7-472A-9DE8-9161737EA1C5")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        interface SVsColorThemeService
        {
        }
    }
}
