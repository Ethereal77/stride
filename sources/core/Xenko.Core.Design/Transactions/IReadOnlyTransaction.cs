// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;

using Xenko.Core.Annotations;

namespace Xenko.Core.Transactions
{
    /// <summary>
    /// An completed interface that cannot be modified anymore, but can be rollbacked or rollforwarded.
    /// </summary>
    public interface IReadOnlyTransaction
    {
        /// <summary>
        /// Gets an unique identifier for the transaction.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Gets the operations executed during the transaction.
        /// </summary>
        [ItemNotNull, NotNull]
        IReadOnlyList<Operation> Operations { get; }

        /// <summary>
        /// Gets the transaction flags.
        /// </summary>
        TransactionFlags Flags { get; }
    }
}
