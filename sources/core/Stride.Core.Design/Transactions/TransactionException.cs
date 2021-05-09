// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.Transactions
{
    /// <summary>
    /// An exception triggered when an invalid operation related to a transaction stack occurs.
    /// </summary>
    public class TransactionException : InvalidOperationException
    {
        /// <summary>
        /// Initializes a new instance of the <seealso cref="TransactionException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public TransactionException(string message)
            : base(message)
        {
        }
    }
}
