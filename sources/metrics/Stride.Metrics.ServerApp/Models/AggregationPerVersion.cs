// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Metrics.ServerApp.Models
{
    /// <summary>
    /// Aggregate data to store metrics results per month and per version.
    /// </summary>
    public class AggregationPerVersion : AggregateBase
    {
        public string Version { get; set; }
    }
}