// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using Stride.Core;
using Stride.Core.Serialization;

namespace Stride.Engine.Design
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
