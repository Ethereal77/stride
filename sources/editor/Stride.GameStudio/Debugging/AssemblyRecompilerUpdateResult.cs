// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Stride.Core.Diagnostics;

namespace Stride.GameStudio.Debugging
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
