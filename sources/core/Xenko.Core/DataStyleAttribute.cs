// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Core
{
    /// <summary>
    /// An attribute to modify the output style of a sequence or mapping. 
    /// This attribute can be apply directly on a type or on a property/field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field)]
    public class DataStyleAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataStyleAttribute"/> class.
        /// </summary>
        /// <param name="style">The style.</param>
        public DataStyleAttribute(DataStyle style)
        {
            this.Style = style;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataStyleAttribute"/> class.
        /// </summary>
        /// <param name="style">The style.</param>
        public DataStyleAttribute(ScalarStyle scalarStyle)
        {
            this.ScalarStyle = scalarStyle;
        }

        /// <summary>
        /// Gets the style.
        /// </summary>
        /// <value>The style.</value>
        public DataStyle Style { get; }

        /// <summary>
        /// Gets the style.
        /// </summary>
        /// <value>The style.</value>
        public ScalarStyle ScalarStyle { get; }
    }
}
