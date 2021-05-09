// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Games
{
    public interface IMessageLoop : IDisposable
    {
        bool NextFrame();
    }
}
