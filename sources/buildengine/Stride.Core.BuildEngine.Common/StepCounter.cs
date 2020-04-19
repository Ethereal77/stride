// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Core.BuildEngine
{
    public class StepCounter
    {
        private readonly int[] stepResults;
        public int Total { get; private set; }

        public StepCounter()
        {
            stepResults = new int[Enum.GetValues(typeof(ResultStatus)).Length];
        }

        public void AddStepResult(ResultStatus result)
        {
            lock (stepResults)
            {
                ++Total;
                ++stepResults[(int)result];
            }
        }

        public int Get(ResultStatus result)
        {
            lock (stepResults)
            {
                return stepResults[(int)result];
            }
        }

        public void Clear()
        {
            lock (stepResults)
            {
                Total = 0;
                foreach (var value in Enum.GetValues(typeof(ResultStatus)))
                    stepResults[(int)value] = 0;
            }
        }
    }
}
