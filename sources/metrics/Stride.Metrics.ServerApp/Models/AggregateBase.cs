// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stride.Metrics.ServerApp.Models
{
    public abstract class AggregateBase
    {
        public int Count { get; set; }
    }
}