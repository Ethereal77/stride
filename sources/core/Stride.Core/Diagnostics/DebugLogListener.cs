// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Diagnostics;

using Xenko.Core.Annotations;

namespace Xenko.Core.Diagnostics
{
    /// <summary>
    /// A <see cref="LogListener"/> implementation redirecting its output to a <see cref="Debug"/>.
    /// </summary>
    public class DebugLogListener : LogListener
    {
        protected override void OnLog([NotNull] ILogMessage logMessage)
        {
            Debug.WriteLine(GetDefaultText(logMessage));
            var exceptionMsg = GetExceptionText(logMessage);
            if (!string.IsNullOrEmpty(exceptionMsg))
            {
                Debug.WriteLine(exceptionMsg);
            }
        }
    }
}
