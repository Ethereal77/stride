// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2009 SLNTools - Christian Warren
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;

using Stride.Core.Annotations;

namespace Stride.Core.VisualStudio
{
    /// <summary>
    /// A section defined in a <see cref="Project"/>
    /// </summary>
    public sealed class Section
    {
        private readonly string name;
        private readonly PropertyItemCollection properties;

        /// <summary>
        /// Initializes a new instance of the <see cref="Section"/> class.
        /// </summary>
        /// <param name="original">The original section to copy from.</param>
        private Section([NotNull] Section original)
            : this(original.Name, original.SectionType, original.Step, original.Properties)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Section"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="sectionType">Type of the section.</param>
        /// <param name="step">The step.</param>
        /// <param name="properties">The property lines.</param>
        public Section([NotNull] string name, string sectionType, string step, IEnumerable<PropertyItem> properties)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            this.name = name;
            SectionType = sectionType;
            Step = step;
            this.properties = new PropertyItemCollection(properties);
        }

        /// <summary>
        /// Gets the name of the section.
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
        /// Gets the properties.
        /// </summary>
        /// <value>The properties.</value>
        public PropertyItemCollection Properties
        {
            get
            {
                return properties;
            }
        }

        /// <summary>
        /// Gets or sets the type of the section.
        /// </summary>
        /// <value>The type of the section.</value>
        public string SectionType { get; set; }

        /// <summary>
        /// Gets or sets the step.
        /// </summary>
        /// <value>The step.</value>
        public string Step { get; set; }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>Section.</returns>
        [NotNull]
        public Section Clone()
        {
            return new Section(this);
        }

        public override string ToString()
        {
            return $"{SectionType} '{Name}'";
        }
    }
}