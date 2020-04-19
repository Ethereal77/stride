// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Core.Transactions
{
    /// <summary>
    /// An interface representing an asynchronous transaction. An asynchronous transaction is a transaction that can be completed asynchronously. It
    /// provides additional safety such as preventing another asynchronous transaction to be created when there is one already in progress.
    /// </summary>
    public interface IAsyncTransaction
    {
    }
}
