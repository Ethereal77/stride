// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Core.Threading
{
    /// <summary>
    /// Interface implemented by pooled closure types through the AssemblyProcessor.
    /// Enables <see cref="PooledDelegateHelper"/> to keep closures and delegates alive.
    /// </summary>
    public interface IPooledClosure
    {
        void AddReference();

        void Release();
    }
}
