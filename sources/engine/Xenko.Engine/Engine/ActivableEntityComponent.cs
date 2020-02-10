// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.ComponentModel;

using Xenko.Core;

namespace Xenko.Engine
{
    /// <summary>
    /// An <see cref="EntityComponent"/> that can be enabled and disabled.
    /// </summary>
    [DataContract]
    public abstract class ActivableEntityComponent : EntityComponent
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="EntityComponent"/> is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        [DataMember(-10)]
        [DefaultValue(true)]
        public virtual bool Enabled { get; set; } = true;
    }
}
