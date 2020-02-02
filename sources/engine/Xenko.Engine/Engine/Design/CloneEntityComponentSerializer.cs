// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using Xenko.Core;
using Xenko.Core.Serialization;

namespace Xenko.Engine.Design
{
    public class CloneEntityComponentSerializer<T> : DataSerializer<T> where T : EntityComponent, new()
    {
        public override void PreSerialize(ref T entityComponent, ArchiveMode mode, SerializationStream stream)
        {
            if (entityComponent == null)
                entityComponent = new T();
        }

        public override void Serialize(ref T entityComponent, ArchiveMode mode, SerializationStream stream)
        {
            if (mode == ArchiveMode.Serialize)
            {
                stream.Write(entityComponent.Entity);
                stream.Write(CloneEntityComponentData.GenerateEntityComponentData(entityComponent));
            }
            else if (mode == ArchiveMode.Deserialize)
            {
                var entity = stream.Read<Entity>();

                var data = stream.Read<CloneEntityComponentData>();
                CloneEntityComponentData.RestoreEntityComponentData(entityComponent, data);
            }
        }
    }
}
