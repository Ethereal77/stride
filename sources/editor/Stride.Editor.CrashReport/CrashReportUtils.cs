// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;
using System.Management;

namespace Stride.Editor.CrashReport
{
    public static class CrashReportUtils
    {
        public static KeyValuePair<string, string> GetOsVersionAndCaption()
        {
            var kvpOsSpecs = new KeyValuePair<string, string>("", "");
            var searcher = new ManagementObjectSearcher("SELECT Caption, Version FROM Win32_OperatingSystem");
            try
            {
                foreach (var os in searcher.Get())
                {
                    var version = os["Version"].ToString();
                    var productName = os["Caption"].ToString();
                    kvpOsSpecs = new KeyValuePair<string, string>(productName, version);
                }
            }
            catch
            {
                // ignored
            }

            return kvpOsSpecs;
        }
    }
}
