// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Stride.Core.Annotations;

namespace Stride.Core
{
    /// <summary>
    ///   Defines the means to identify an object with a <see cref="Guid"/>.
    /// </summary>
    public interface IIdentifiable
    {
        /// <summary>
        ///   Gets or sets the unique identifier of this instance.
        /// </summary>
        [NonOverridable]
        Guid Id { get; set; }
    }
}
