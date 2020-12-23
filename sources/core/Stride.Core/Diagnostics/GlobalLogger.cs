// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Stride.Core.Annotations;

namespace Stride.Core.Diagnostics
{
    /// <summary>
    ///   Represents an <see cref="ILogger"/> that redirects messages to a global handler and
    ///   associates itself to the name of a module.
    /// </summary>
    public sealed class GlobalLogger : Logger
    {
        /// <summary>
        ///   Dictionary of instantiated loggers by module name.
        /// </summary>
        private static readonly Dictionary<string, Logger> g_LoggersByModuleName = new Dictionary<string, Logger>();


        private GlobalLogger(string module)
        {
            Module = module;
        }

        public delegate void MessageFilterDelegate(ref ILogMessage logMessage);

        /// <summary>
        ///   Event raised before a message is logged. Used to filter log messages before they are written.
        /// </summary>
        public static event MessageFilterDelegate GlobalMessageFilter;

        /// <summary>
        ///   Event raised when a message is logged.
        /// </summary>
        public static event Action<ILogMessage> GlobalMessageLogged;

        /// <summary>
        ///   Gets all registered loggers.
        /// </summary>
        /// <value>The registered loggers.</value>
        [NotNull]
        public static Logger[] RegisteredLoggers
        {
            get
            {
                lock (g_LoggersByModuleName)
                {
                    var loggers = new Logger[g_LoggersByModuleName.Count];
                    g_LoggersByModuleName.Values.CopyTo(loggers, 0);
                    return loggers;
                }
            }
        }

        /// <summary>
        ///   Activates all the loggers using the specified action.
        /// </summary>
        /// <param name="activator">An action to perform for each log.</param>
        /// <exception cref="ArgumentNullException"><paramref name="activator"/> is a <c>null</c> reference.</exception>
        public static void ActivateLog([NotNull] Action<Logger> activator)
        {
            if (activator is null)
                throw new ArgumentNullException(nameof(activator));

            foreach (var logger in g_LoggersByModuleName.Values)
                activator(logger);
        }

        /// <summary>
        ///   Activates the loggers that match a pattern on the module name.
        /// </summary>
        /// <param name="regexPatternModule">The regex pattern to match a module name.</param>
        /// <param name="minimumLevel">The minimum log level.</param>
        /// <param name="maximumLevel">The maximum log level.</param>
        /// <param name="enabledFlag"><c>true</c> to enaable the logs; <c>false</c> to disable them.</param>
        /// <exception cref="ArgumentNullException"><paramref name="regexPatternModule"/> is a <c>null</c> reference.</exception>
        public static void ActivateLog([NotNull] string regexPatternModule, LogMessageType minimumLevel, LogMessageType maximumLevel = LogMessageType.Fatal, bool enabledFlag = true)
        {
            if (regexPatternModule is null)
                throw new ArgumentNullException(nameof(regexPatternModule));

            var regex = new Regex(regexPatternModule);
            ActivateLog(regex, minimumLevel, maximumLevel, enabledFlag);
        }

        /// <summary>
        ///   Activates the loggers that match a pattern on the module name.
        /// </summary>
        /// <param name="regexPatternModule">The regex pattern to match a module name.</param>
        /// <param name="minimumLevel">The minimum log level.</param>
        /// <param name="maximumLevel">The maximum log level.</param>
        /// <param name="enabledFlag"><c>true</c> to enaable the logs; <c>false</c> to disable them.</param>
        /// <exception cref="ArgumentNullException"><paramref name="regexPatternModule"/> is a <c>null</c> reference.</exception>
        public static void ActivateLog([NotNull] Regex regexPatternModule, LogMessageType minimumLevel, LogMessageType maximumLevel = LogMessageType.Fatal, bool enabledFlag = true)
        {
            if (regexPatternModule is null)
                throw new ArgumentNullException(nameof(regexPatternModule));

            ActivateLog(
                logger =>
                    {
                        if (regexPatternModule.Match(logger.Module).Success)
                        {
                            logger.ActivateLog(minimumLevel, maximumLevel, enabledFlag);
                        }
                    });
        }

        /// <summary>
        ///   Gets the <see cref="Logger"/> associated to the specified module.
        /// </summary>
        /// <param name="module">The module name.</param>
        /// <returns>A <see cref="Logger"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="module"/> is a <c>null</c> reference.</exception>
        public static Logger GetLogger([NotNull] string module)
        {
            return GetLogger(module, MinimumLevelEnabled);
        }

        /// <summary>
        ///   Gets the <see cref="Logger"/> associated to the specified module.
        /// </summary>
        /// <param name="module">The module name.</param>
        /// <param name="minimumLevel">Minimum log level (only applied if a new logger is created).</param>
        /// <returns>A <see cref="Logger"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="module"/> is a <c>null</c> reference.</exception>
        public static Logger GetLogger([NotNull] string module, LogMessageType minimumLevel)
        {
            if (module is null)
                throw new ArgumentNullException(nameof(module));

            lock (g_LoggersByModuleName)
            {
                if (g_LoggersByModuleName.TryGetValue(module, out Logger logger))
                    return logger;

                logger = new GlobalLogger(module);
                logger.ActivateLog(minimumLevel);
                g_LoggersByModuleName.Add(module, logger);

                return logger;
            }
        }

        protected override void LogRaw(ILogMessage logMessage)
        {
            var filterHandler = GlobalMessageFilter;
            if (filterHandler != null)
            {
                filterHandler(ref logMessage);
                if (logMessage is null)
                    return;
            }

            GlobalMessageLogged?.Invoke(logMessage);
        }
    }
}
