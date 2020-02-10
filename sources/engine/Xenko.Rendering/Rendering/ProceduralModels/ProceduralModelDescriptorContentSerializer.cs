// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Core;
using Xenko.Core.Serialization;
using Xenko.Core.Serialization.Contents;
using Xenko.Graphics;

namespace Xenko.Rendering.ProceduralModels
{
    internal class ProceduralModelDescriptorContentSerializer : ContentSerializerBase<Model>
    {
        private static readonly DataContentSerializerHelper<ProceduralModelDescriptor> DataSerializerHelper = new DataContentSerializerHelper<ProceduralModelDescriptor>();

        public override Type SerializationType
        {
            get { return typeof(ProceduralModelDescriptor); }
        }

        public override void Serialize(ContentSerializerContext context, SerializationStream stream, Model model)
        {
            var proceduralModel = new ProceduralModelDescriptor();
            DataSerializerHelper.Serialize(context, stream, proceduralModel);

            var services = stream.Context.Tags.Get(ServiceRegistry.ServiceRegistryKey);

            proceduralModel.GenerateModel(services, model);
        }
    }
}
