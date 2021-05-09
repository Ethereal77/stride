// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stride.Metrics.ServerApp.Models
{
    /// <summary>
    /// A base class that provides a automatically <see cref="Created"/> field
    /// </summary>
    public abstract class MetricCreatedBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MetricInstall"/> class.
        /// </summary>
        protected MetricCreatedBase()
        {
            Created = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets or sets when this instance was created.
        /// </summary>
        /// <value>The datetime when this instance was created.</value>
        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime Created { get; set; }
    }
}