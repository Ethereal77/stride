// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Core
{
    /// <summary>
    ///   Provides a mechanism for releasing managed resources.
    /// </summary>
    /// <remarks>
    ///   This interface is similar in purpose to <see cref="System.IDisposable"/>, but it only deals with managed resources.
    ///   <para/>
    ///   Classes implementing both <see cref="IDestroyable"/> and <see cref="System.IDisposable"/> should call
    ///   <see cref="Destroy"/> from the <see cref="System.IDisposable.Dispose"/> method when appropriate.
    /// </remarks>
    public interface IDestroyable
    {
        /// <summary>
        ///   Performs application-defined tasks associated with freeing, releasing, or resetting managed resources.
        /// </summary>
        void Destroy();
    }
}
