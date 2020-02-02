// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Engine
{
    /// <summary>
    /// Defines the order of a component type. This information is used to determine the most "important" component of a collection
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ComponentOrderAttribute : EntityComponentAttributeBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentOrderAttribute"/> class.
        /// </summary>
        /// <param name="order">The order of the associated component.</param>
        public ComponentOrderAttribute(int order)
        {
            Order = order;
        }

        /// <summary>
        /// Gets the order of the associated component.
        /// </summary>
        public int Order { get; }
    }
}
