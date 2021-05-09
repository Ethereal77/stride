// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Diagnostics;

namespace Stride.Core.BuildEngine
{
    public class BuildStepEventArgs : EventArgs
    {
        public BuildStepEventArgs(BuildStep step, ILogger logger)
        {
            Step = step;
            Logger = logger;
        }

        public BuildStep Step { get; private set; }

        public ILogger Logger { get; set; }
    }
}
