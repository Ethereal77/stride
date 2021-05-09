// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Stride.Core.Annotations;
using Stride.Core.Transactions;
using Stride.Core.Presentation.Dirtiables;

namespace Stride.Core.Assets.Editor.ViewModel
{
    internal class AssetsUpgradeOperation : Operation, IDirtyingOperation
    {
        public AssetsUpgradeOperation([NotNull] IDirtiable dirtiable)
        {
            if (dirtiable == null) throw new ArgumentNullException(nameof(dirtiable));
            Dirtiables = new [] {dirtiable};
        }

        /// <inheritdoc/>
        public bool IsDone => true;

        /// <inheritdoc/>
        public IReadOnlyList<IDirtiable> Dirtiables { get; }

        /// <inheritdoc/>
        protected override void Rollback()
        {
            // Intentionally does nothing
        }

        /// <inheritdoc/>
        protected override void Rollforward()
        {
            // Intentionally does nothing
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{{{nameof(AssetsUpgradeOperation)}: {Dirtiables[0]}";
        }
    }
}
