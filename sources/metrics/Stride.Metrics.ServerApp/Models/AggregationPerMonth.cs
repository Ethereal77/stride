// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Metrics.ServerApp.Models
{
    /// <summary>
    /// Aggregate data to store metrics results per month.
    /// </summary>
    public class AggregationPerMonth : AggregateBase
    {
        public int Year { get; set; }

        public int Month { get; set; }
    }

    public class AggregationPerDays: AggregateBase
    {
        public int Year { get; set; }

        public int Month { get; set; }

        public int Day { get; set; }
    }
}