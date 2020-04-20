// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Core.Serialization
{
    public abstract class ClassDataSerializer<T> : DataSerializer<T> where T : class, new()
    {
        /// <inheritdoc/>
        public override void PreSerialize(ref T obj, ArchiveMode mode, SerializationStream stream)
        {
            if (mode == ArchiveMode.Deserialize && obj == null)
            {
                try
                {
                    obj = new T();
                }
                catch (System.Exception)
                {
                    //obj = (T)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(T));
                    //return;
                    throw;
                }
            }
        }
    }
}
