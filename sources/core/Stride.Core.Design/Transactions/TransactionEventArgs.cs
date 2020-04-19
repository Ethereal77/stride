// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Core.Transactions
{
    /// <summary>
    /// Arguments of events triggered by <see cref="ITransactionStack"/> instances that affect a single transaction.
    /// </summary>
    public class TransactionEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionEventArgs"/> class.
        /// </summary>
        /// <param name="transaction">The transaction associated to this event.</param>
        public TransactionEventArgs(IReadOnlyTransaction transaction)
        {
            Transaction = transaction;
        }

        /// <summary>
        /// Gets the transaction associated to this event.
        /// </summary>
        public IReadOnlyTransaction Transaction { get; }
    }
}
