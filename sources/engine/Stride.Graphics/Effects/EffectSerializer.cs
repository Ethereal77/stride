// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core;
using Stride.Core.Serialization;
using Stride.Shaders;

namespace Stride.Graphics
{
    internal class EffectSerializer : DataSerializer<Effect>
    {
        public override void Serialize(ref Effect effect, ArchiveMode mode, SerializationStream stream)
        {
            if (mode == ArchiveMode.Serialize)
                throw new InvalidOperationException();

            var services = stream.Context.Tags.Get(ServiceRegistry.ServiceRegistryKey);
            var graphicsDeviceService = services.GetSafeServiceAs<IGraphicsDeviceService>();

            var effectBytecode = stream.Read<EffectBytecode>();

            if (effect == null)
                effect = new Effect();

            effect.InitializeFrom(graphicsDeviceService.GraphicsDevice, effectBytecode);
        }
    }
}
