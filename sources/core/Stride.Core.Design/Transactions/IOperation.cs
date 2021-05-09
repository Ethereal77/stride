// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Transactions
{
    /// <summary>
    /// An internal interface to interact with <see cref="Operation"/> instances from a <see cref="TransactionStack"/>.
    /// </summary>
    internal interface IOperation
    {
        /// <summary>
        /// Freezes the operation, preventing it to be rollbacked or rollforwarded again.
        /// </summary>
        /// <remarks>This operation should release any reference that is not needed anymore by the operation.</remarks>
        void Freeze();

        /// <summary>
        /// Rollbacks the operation, restoring the state of object as they were before the operation.
        /// </summary>
        void Rollback();

        /// <summary>
        /// Rollforwards the operation, restoring the state of object as they were after the operation.
        /// </summary>
        void Rollforward();
    }
}
