// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Stride.Core.Annotations;

namespace Stride.Engine.Design
{
    /// <summary>
    ///   An attribute used to associate a default <see cref="EntityProcessor"/> to an <see cref="EntityComponent"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DefaultEntityComponentProcessorAttribute : DynamicTypeAttributeBase
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="DefaultEntityComponentProcessorAttribute"/> class.
        /// </summary>
        /// <param name="type">
        ///   The type of <see cref="EntityProcessor"/> to use by default as processor for components of tha annotated type.
        /// </param>
        public DefaultEntityComponentProcessorAttribute(Type type) : base(type) { }


        /// <summary>
        ///   Gets or sets the mode where the <see cref="EntityProcessor"/> will be executing.
        /// </summary>
        public ExecutionMode ExecutionMode { get; set; } = ExecutionMode.All;
    }
}
