// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Metrics.ServerApp.Models
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