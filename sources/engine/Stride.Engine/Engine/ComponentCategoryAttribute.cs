// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Stride.Engine
{
    /// <summary>
    /// Defines the category of a component type. This information is used to group component types in Game Studio.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ComponentCategoryAttribute : EntityComponentAttributeBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentCategoryAttribute"/> class.
        /// </summary>
        /// <param name="category">The category of the associated component.</param>
        /// <exception cref="ArgumentNullException">If category is null or empty.</exception>
        public ComponentCategoryAttribute(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                throw new ArgumentNullException(nameof(category));
            }

            Category = category;
        }

        /// <summary>
        /// Gets the category of the associated component
        /// </summary>
        public string Category { get; }

        /// <summary>
        /// Gets the category for the component type.
        /// </summary>
        /// <param name="type">The type to get the attribute from</param>
        /// <returns>The category.</returns>
        public static string GetCategory(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.GetCustomAttribute<ComponentCategoryAttribute>()?.Category;
        }
    }
}
