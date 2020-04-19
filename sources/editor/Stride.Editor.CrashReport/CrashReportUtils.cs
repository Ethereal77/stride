// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;
using System.Management;

namespace Xenko.Editor.CrashReport
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
