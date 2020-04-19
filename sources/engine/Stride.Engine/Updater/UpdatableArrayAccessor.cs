// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Updater
{
    /// <summary>
    /// Defines how to get and set an array value for the <see cref="UpdateEngine"/>.
    /// </summary>
    public class UpdatableArrayAccessor<T> : UpdatableField<T>
    {
        public UpdatableArrayAccessor(int index) : base(0)
        {
            Offset = UpdateEngineHelper.ArrayFirstElementOffset + index * Size;
        }

        /// <inheritdoc/>
        public override EnterChecker CreateEnterChecker()
        {
            // Compute index
            var index = (Offset - UpdateEngineHelper.ArrayFirstElementOffset) / Size;

            // Expect an array at least index + 1 items
            return new ListEnterChecker<T>(index + 1);
        }
    }
}
