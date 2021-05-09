// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stride.Metrics
{
    internal class NewMetricMessage
    {
        public const int MaxValueLength = 256;

        private string value;

        public Guid ApplicationId { get; set; }

        public Guid InstallId { get; set; }

        public int SessionId { get; set; }

        public int EventId { get; set; }

        public Guid SpecialId { get; set; }

        public Guid MetricId { get; set; }

        public string Value
        {
            get { return value; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (value.Length > MaxValueLength)
                {
                    throw new ArgumentOutOfRangeException("value", "string length must be <= " + MaxValueLength + " characters");
                }
                this.value = value;
            }
        }
    }
}
