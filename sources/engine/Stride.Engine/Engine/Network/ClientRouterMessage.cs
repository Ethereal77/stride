// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Engine.Network
{
    /// <summary>
    /// Message exchanged between client and router.
    /// Note: shouldn't collide with <see cref="RouterMessage"/>.
    /// </summary>
    public enum ClientRouterMessage : ushort
    {
        RequestServer = 0x0000, // ClientRequestServer <string:url>
        ServerStarted = 0x0001, // ClientServerStarted <int:errorcode> <string:message>
    }
}
