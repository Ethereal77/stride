// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Diagnostics;

namespace Stride.Core.BuildEngine
{
    public class BuildStepLogger : Logger
    {
        private readonly BuildStep buildStep;
        private readonly ILogger mainLogger;
        public readonly TimestampLocalLogger StepLogger;

        public BuildStepLogger(BuildStep buildStep, ILogger mainLogger, DateTime startTime)
        {
            this.buildStep = buildStep;
            this.mainLogger = mainLogger;
            StepLogger = new TimestampLocalLogger(startTime);
            // Let's receive all level messages, each logger will filter them itself
            ActivateLog(LogMessageType.Debug);
            // StepLogger messages will be forwarded to the monitor, which will also filter itself
            StepLogger.ActivateLog(LogMessageType.Debug);
        }

        protected override void LogRaw(ILogMessage logMessage)
        {
            buildStep.Logger.Log(logMessage);

            mainLogger?.Log(logMessage);
            StepLogger?.Log(logMessage);
        }
    }
}
