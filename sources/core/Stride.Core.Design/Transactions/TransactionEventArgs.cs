// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.Transactions
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
