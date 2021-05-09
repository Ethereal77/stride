// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Graphics.Regression
{
    public enum TestRunnerMessageType
    {
        Unknown = 0,
        TestStarted = 1,
        TestFinished = 2,
        TestOutput = 3,
        SessionSuccess = 4,
        SessionFailure = 5,
    }
}
