// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;

using Xenko.Core.Diagnostics;

namespace Xenko.GameStudio.Debugging
{
    partial class AssemblyRecompiler
    {
        public class UpdateResult : LoggerResult
        {
            private readonly ILogger logger;

            public UpdateResult(ILogger logger)
            {
                this.logger = logger;
                UnloadedProjects = new List<SourceGroup>();
                LoadedProjects = new List<SourceGroup>();
            }

            /// <summary>
            /// Gets the projects to unload.
            /// </summary>
            /// <value>
            /// The projects to unload.
            /// </value>
            public List<SourceGroup> UnloadedProjects { get; private set; }

            /// <summary>
            /// Gets the projects to load.
            /// </summary>
            /// <value>
            /// The projects to load.
            /// </value>
            public List<SourceGroup> LoadedProjects { get; private set; }

            protected override void LogRaw(ILogMessage logMessage)
            {
                base.LogRaw(logMessage);
                logger.Log(logMessage);
            }
        }
    }
}
