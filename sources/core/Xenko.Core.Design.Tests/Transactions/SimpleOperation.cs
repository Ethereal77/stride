// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Core.Transactions;

namespace Xenko.Core.Design.Tests.Transactions
{
    internal class SimpleOperation : Operation
    {
        public Guid Guid { get; } = Guid.NewGuid();

        public bool IsDone { get; private set; } = true;

        public int RollbackCount { get; private set; }

        public int RollforwardCount { get; private set; }

        protected override void Rollback()
        {
            IsDone = false;
            ++RollbackCount;
        }

        protected override void Rollforward()
        {
            IsDone = true;
            ++RollforwardCount;
        }
    }
}
