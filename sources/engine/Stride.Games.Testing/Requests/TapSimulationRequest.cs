// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core;
using Stride.Core.Mathematics;
using Stride.Input;

namespace Stride.Games.Testing.Requests
{
    [DataContract]
    internal class TapSimulationRequest : TestRequestBase
    {
        public PointerEventType EventType;
        public TimeSpan Delta;
        public Vector2 Coords;
        public Vector2 CoordsDelta;
    }
}
