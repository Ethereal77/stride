// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Metrics
{
    /// <summary>
    /// Identifies a metric application.
    /// </summary>
    public sealed class MetricAppId
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MetricAppId"/> class.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentNullException">name</exception>
        public MetricAppId(Guid guid, string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            Guid = guid;
            Name = name;
        }

        /// <summary>
        /// The unique identifier of this application.
        /// </summary>
        public readonly Guid Guid;

        /// <summary>
        /// The name of this application.
        /// </summary>
        public readonly string Name;
    }
}
