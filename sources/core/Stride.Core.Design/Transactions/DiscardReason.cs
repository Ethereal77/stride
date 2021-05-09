// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Transactions
{
    /// <summary>
    /// A enum listing the possible reasons for transactions to be discarded from an <see cref="ITransactionStack"/>.
    /// </summary>
    public enum DiscardReason
    {
        /// <summary>
        /// Transactions have been discarded because the stack is full.
        /// </summary>
        StackFull,
        /// <summary>
        /// Transactions have been discarded because the top of the stack has been purged.
        /// </summary>
        StackPurged,
    }
}
