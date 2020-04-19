// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Updater
{
    /// <summary>
    /// Defines an update compiled by <see cref="UpdateEngine.Compile"/>
    /// for subsequent uses by <see cref="UpdateEngine.Run"/>.
    /// </summary>
    public struct CompiledUpdate
    {
        /// <summary>
        /// Stores the list of update operations.
        /// </summary>
        internal UpdateOperation[] UpdateOperations;

        /// <summary>
        /// Stores the list of pre-allocated objects for non-blittable struct unboxing.
        /// </summary>
        internal object[] TemporaryObjects;
    }
}
