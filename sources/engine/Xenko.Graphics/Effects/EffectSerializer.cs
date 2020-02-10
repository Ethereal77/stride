// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Core;
using Xenko.Core.Serialization;
using Xenko.Shaders;

namespace Xenko.Graphics
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
