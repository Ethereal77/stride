// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Stride.Metrics.ServerApp.Models
{
    /// <summary>
    /// Aggregate data to store metrics results per day.
    /// </summary>
    public class AggregationPerDay : AggregateBase
    {
        public DateTime Date { get; set; }
    }
}