// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Diagnostics;

namespace Stride.Metrics
{
    [DebuggerDisplay("Metric [{Name} : {MetridId}]")]
    internal class NewMetricDefinitionMessage
    {
        public Guid MetridId { get; set; }

        public string Name { get; set; }
    }
}