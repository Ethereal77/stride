// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;

using Stride.LauncherApp.Services;
using Stride.Editor.CrashReport;

namespace Stride.LauncherApp
{
    internal class CrashReportSettings : ICrashEmailSetting
    {
        public CrashReportSettings()
        {
            Email = GameStudioSettings.CrashReportEmail;
            StoreCrashEmail = !string.IsNullOrEmpty(Email);
        }

        public bool StoreCrashEmail { get; set; }

        public string Email { get; set; }

        public void Save()
        {
            GameStudioSettings.CrashReportEmail = Email;
        }
    }
}
