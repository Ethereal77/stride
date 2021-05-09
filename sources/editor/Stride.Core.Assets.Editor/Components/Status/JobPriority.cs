// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Assets.Editor.Components.Status
{
    /// <summary>
    /// This enum describes the priority of a job.
    /// </summary>
    public enum JobPriority
    {
        /// <summary>
        /// A background task of the application
        /// </summary>
        Background,
        /// <summary>
        /// An editor of the application that is working
        /// </summary>
        Editor,
        /// <summary>
        /// A major compilation process in the application
        /// </summary>
        Compile,
    }
}
