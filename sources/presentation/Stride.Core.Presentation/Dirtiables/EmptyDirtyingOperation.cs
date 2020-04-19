// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;

using Xenko.Core.Annotations;

namespace Xenko.Core.Presentation.Dirtiables
{
    public sealed class EmptyDirtyingOperation : DirtyingOperation
    {
        public EmptyDirtyingOperation([NotNull] IEnumerable<IDirtiable> dirtiables)
            : base(dirtiables)
        {
        }

        /// <inheritdoc/>
        protected override void Undo()
        {
        }

        /// <inheritdoc/>
        protected override void Redo()
        {
        }
    }
}
