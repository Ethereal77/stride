// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2009 SLNTools - Christian Warren
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Annotations;

namespace Stride.Core.VisualStudio
{
    /// <summary>
    /// A key/value pair used by <see cref="PropertyItemCollection" />
    /// </summary>
    public sealed class PropertyItem
    {
        private readonly string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyItem"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public PropertyItem([NotNull] string name, string value)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            this.name = name;
            Value = value;
        }

        private PropertyItem([NotNull] PropertyItem original)
            : this(original.Name, original.Value)
        {
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>PropertyItem.</returns>
        [NotNull]
        public PropertyItem Clone()
        {
            return new PropertyItem(this);
        }

        public override string ToString()
        {
            return $"{Name} = {Value}";
        }
    }
}