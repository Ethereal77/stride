// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Serialization;
using Xenko.Core.Serialization.Serializers;

namespace Xenko.Core.Collections
{
    /// <summary>
    /// A list to ensure that all items are not null.
    /// </summary>
    /// <typeparam name="T">Type of the item</typeparam>
    [DataSerializer(typeof(ListAllSerializer<,>), Mode = DataSerializerGenericMode.TypeAndGenericArguments)]
    public class SafeList<T> : ConstrainedList<T> where T : class
    {
        private const string ExceptionError = "The item added to the list cannot be null";

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeList{T}"/> class.
        /// </summary>
        public SafeList()
            : base(NonNullConstraint, true, ExceptionError)
        {
        }

        private static bool NonNullConstraint(ConstrainedList<T> constrainedList, T arg2)
        {
            return arg2 != null;
        }
    }
}
