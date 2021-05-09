// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.Transactions
{
    [Flags]
    public enum TransactionFlags
    {
        None = 0,

        /// <summary>
        /// Keep parent transaction alive (useful to start async inner transactions).
        /// </summary>
        KeepParentsAlive = 1,
    }
}
