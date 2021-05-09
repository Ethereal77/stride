// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Annotations;
using Stride.Core.Settings;

namespace Stride.Core.Diagnostics
{
    /// <summary>
    ///   Base implementation for <see cref="ILogger"/>. Provides a mechanism to register logging messages and
    ///   information about the functioning of a process, program or operation.
    /// </summary>
    public abstract partial class Logger : ILogger
    {
        private static readonly object _lock = new object();

        private static LogMessageType? minimumLevelEnabled;

        /// <summary>
        ///   Gets the minimum log level enabled from the configuration.
        /// </summary>
        public static LogMessageType MinimumLevelEnabled
        {
            get
            {
                if (minimumLevelEnabled.HasValue)
                    return minimumLevelEnabled.Value;

                var loggerConfig = AppSettingsManager.Settings.GetSettings<LoggerConfig>();
                minimumLevelEnabled = loggerConfig?.Level ?? LogMessageType.Info; // Default value
                return minimumLevelEnabled.Value;
            }

            set => minimumLevelEnabled = value;
        }

        /// <summary>
        ///   Value that indicates whether the debug message logging is enabled at a global level.
        /// </summary>
        public static readonly bool IsDebugEnabled = MinimumLevelEnabled <= LogMessageType.Debug;
        /// <summary>
        ///   Value that indicates whether the verbose message logging is enabled at a global level.
        /// </summary>
        public static readonly bool IsVerboseEnabled = MinimumLevelEnabled <= LogMessageType.Verbose;

        protected readonly bool[] EnableTypes;


        /// <summary>
        ///   Gets the module name.
        /// </summary>
        /// <value>The module name.</value>
        /// <remarks>
        ///   The module is an identifier for a logical part of the system. It can be a class name, a namespace or a regular string
        ///   not linked to a code hierarchy.
        /// </remarks>
        public string Module { get; protected internal set; }

        /// <summary>
        ///   Gets or sets a value indicating whether this instance has logged any error.
        /// </summary>
        /// <value><c>true</c> if this instance has errors; otherwise, <c>false</c>.</value>
        public bool HasErrors { get; set; }


        /// <summary>
        ///   Occurs when a message is logged.
        /// </summary>
        public event EventHandler<MessageLoggedEventArgs> MessageLogged;


        /// <summary>
        ///   Initializes a new instance of the <see cref="Logger" /> class.
        /// </summary>
        protected Logger()
        {
            EnableTypes = new bool[(int) LogMessageType.Fatal + 1];
        }


        /// <summary>
        ///   Activates the log for this logger for a range of <see cref="LogMessageType"/>.
        /// </summary>
        /// <param name="fromLevel">The lowest inclusive level to log for.</param>
        /// <param name="toLevel">The highest inclusive level to log for.</param>
        /// <param name="enabled"><c>true</c> to enable the log, <c>false</c> otherwise. Default is <c>true</c>.</param>
        /// <remarks>
        ///   Outside the specified range of message types, the logging is either disabled (if <paramref name="enabled"/> is <c>true</c>),
        ///   or enabled (if <paramref name="enabled"/> is <c>false</c>).
        /// </remarks>
        public void ActivateLog(LogMessageType fromLevel, LogMessageType toLevel = LogMessageType.Fatal, bool enabled = true)
        {
            // From lower to higher, so we keep fromLevel < toLevel
            if (fromLevel > toLevel)
            {
                (fromLevel, toLevel) = (toLevel, fromLevel);
            }

            for (var i = 0; i < EnableTypes.Length; i++)
                EnableTypes[i] = (i >= (int) fromLevel && i <= (int) toLevel) ? enabled : !enabled;
        }

        /// <summary>
        ///   Activates the log for this logger for a specific <see cref="LogMessageType"/>.
        /// </summary>
        /// <param name="type">The type of message to enable or disable. All other types are leaved intact.</param>
        /// <param name="enabled"><c>true</c> to enable the log, <c>false</c> otherwise. Default is <c>true</c>.</param>
        public void ActivateLog(LogMessageType type, bool enabled)
        {
            EnableTypes[(int) type] = enabled;
        }

        /// <summary>
        ///   Determines whether a particular <see cref="LogMessageType"/> is activated.
        /// </summary>
        /// <param name="type">The type of message.</param>
        /// <returns><c>true</c> if the log is activated, otherwise <c>false</c>.</returns>
        public bool IsActivated(LogMessageType type)
        {
            return EnableTypes[(int) type];
        }

        public void Log([NotNull] ILogMessage logMessage)
        {
            if (logMessage is null)
                throw new ArgumentNullException(nameof(logMessage));

            lock (_lock)
            {
                // Even if the type is not enabled, set HasErrors to know that there is an error even if not logged
                if (logMessage.Type == LogMessageType.Error || logMessage.Type == LogMessageType.Fatal)
                    HasErrors = true;

                // Only log when a particular type is enabled
                if (EnableTypes[(int) logMessage.Type])
                {
                    LogRaw(logMessage);
                    MessageLogged?.Invoke(this, new MessageLoggedEventArgs(logMessage));
                }
            }
        }

        /// <summary>
        ///   Internal method used to log a message. All <c>Info</c>, <c>Debug</c>, <c>Error</c>, etc. methods are calling this method.
        /// </summary>
        /// <param name="logMessage">The log message.</param>
        protected abstract void LogRaw(ILogMessage logMessage);
    }
}
