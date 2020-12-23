// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Stride.Core;
using Stride.Core.Collections;
using Stride.Core.Extensions;
using Stride.Core.Serialization;

namespace Stride.Rendering
{
    /// <summary>
    ///   Represents a class that can manage a collection of effect parameters (resources and data).
    ///   A specific data and resource layout can be forced (usually by the consuming effect).
    /// </summary>
    [DataSerializer(typeof(ParameterCollection.Serializer))]
    [DataSerializerGlobal(null, typeof(FastList<ParameterKeyInfo>))]
    [DebuggerTypeProxy(typeof(ParameterCollection.DebugView))]
    public class ParameterCollection
    {
        private static readonly byte[] EmptyData = new byte[0];

        private ParameterCollectionLayout layout;

        // TODO: Switch to FastListStruct (for serialization)
        private FastList<ParameterKeyInfo> parameterKeyInfos = new FastList<ParameterKeyInfo>(4);

        // Constants and resources
        [DataMemberIgnore]
        public byte[] DataValues = EmptyData;
        [DataMemberIgnore]
        public object[] ObjectValues;

        [DataMemberIgnore]
        public int PermutationCounter = 1;

        [DataMemberIgnore]
        public int LayoutCounter = 1;

        [DataMemberIgnore]
        public FastList<ParameterKeyInfo> ParameterKeyInfos => parameterKeyInfos;

        [DataMemberIgnore]
        public ParameterCollectionLayout Layout => layout;

        [DataMemberIgnore]
        public bool HasLayout => layout != null;

        public ParameterCollection() { }

        public unsafe ParameterCollection(ParameterCollection parameterCollection)
        {
            // Copy layout
            if (parameterCollection.layout != null)
            {
                layout = parameterCollection.layout;
            }

            // Copy parameter keys
            parameterKeyInfos.AddRange(parameterCollection.parameterKeyInfos);

            // Copy objects
            if (parameterCollection.ObjectValues != null)
            {
                ObjectValues = new object[parameterCollection.ObjectValues.Length];
                for (int i = 0; i < ObjectValues.Length; ++i)
                    ObjectValues[i] = parameterCollection.ObjectValues[i];
            }

            // Copy data
            if (parameterCollection.DataValues != null)
            {
                DataValues = new byte[parameterCollection.DataValues.Length];
                fixed (byte* dataValuesSources = parameterCollection.DataValues)
                fixed (byte* dataValuesDest = DataValues)
                {
                    Utilities.CopyMemory((IntPtr) dataValuesDest, (IntPtr) dataValuesSources, DataValues.Length);
                }
            }
        }

        public override string ToString()
        {
            var parameterKeysByType = ParameterKeyInfos.GroupBy(x => x.Key.Type);
            return "ParameterCollection: " +
                   string.Join(", ", parameterKeysByType.Select(x => $"{x.Count()} {x.Key}(s)"));
        }

        /// <summary>
        ///   Gets an accessor to get and set objects more quickly.
        /// </summary>
        /// <typeparam name="T">Type of the parameter to access.</typeparam>
        /// <param name="parameterKey">Key of the parameter to access.</param>
        /// <param name="createIfNew">
        ///   Value indicating whether to create a new accesor if none exists already.
        /// </param>
        /// <returns>An object that can be used to get and set the parameter quickly.</returns>
        public ObjectParameterAccessor<T> GetAccessor<T>(ObjectParameterKey<T> parameterKey, bool createIfNew = true)
        {
            var accessor = GetObjectParameterHelper(parameterKey, createIfNew);
            return new ObjectParameterAccessor<T>(accessor.Offset, accessor.Count);
        }

        /// <summary>
        ///   Gets an accessor to get and set effect permutations more quickly.
        /// </summary>
        /// <typeparam name="T">Type of the permutation parameter to access.</typeparam>
        /// <param name="parameterKey">Key of the parameter to access.</param>
        /// <param name="createIfNew">
        ///   Value indicating whether to create a new accesor if none exists already.
        /// </param>
        /// <returns>An object that can be used to get and set the permutation parameter quickly.</returns>
        public PermutationParameter<T> GetAccessor<T>(PermutationParameterKey<T> parameterKey, bool createIfNew = true)
        {
            // Remap it as PermutationParameter
            var accessor = GetObjectParameterHelper(parameterKey, createIfNew);
            return new PermutationParameter<T>(accessor.Offset, accessor.Count);
        }

        /// <summary>
        ///   Gets an accessor to get and set values more quickly.
        /// </summary>
        /// <typeparam name="T">Type of the value parameter to access.</typeparam>
        /// <param name="parameterKey">Key of the parameter to access.</param>
        /// <param name="elementCount">Number of contiguous values to access. For scalar values it is 1 by default.</param>
        /// <returns>An object that can be used to get and set the value parameter quickly.</returns>
        public ValueParameter<T> GetAccessor<T>(ValueParameterKey<T> parameterKey, int elementCount = 1)
            where T : struct
        {
            var accessor = GetValueAccessorHelper(parameterKey, elementCount);
            return new ValueParameter<T>(accessor.Offset, accessor.Count);
        }

        private unsafe Accessor GetValueAccessorHelper(ParameterKey parameterKey, int elementCount = 1)
        {
            // Find existing first
            for (int i = 0; i < parameterKeyInfos.Count; ++i)
            {
                if (parameterKeyInfos.Items[i].Key == parameterKey)
                {
                    return parameterKeyInfos.Items[i].GetValueAccessor();
                }
            }

            LayoutCounter++;

            // Check layout if it exists
            if (layout != null)
            {
                foreach (var layoutParameterKeyInfo in layout.LayoutParameterKeyInfos)
                {
                    if (layoutParameterKeyInfo.Key == parameterKey)
                    {
                        parameterKeyInfos.Add(layoutParameterKeyInfo);
                        return layoutParameterKeyInfo.GetValueAccessor();
                    }
                }
            }

            // Compute size
            var elementSize = parameterKey.Size;
            var totalSize = elementSize;
            if (elementCount > 1)
                totalSize += (elementSize + 15) / 16 * 16 * (elementCount - 1);

            // Create offset entry
            var memberOffset = DataValues.Length;
            parameterKeyInfos.Add(new ParameterKeyInfo(parameterKey, memberOffset, elementCount));

            // We append at the end; resize array to accomodate new data
            Array.Resize(ref DataValues, DataValues.Length + totalSize);

            // Initialize default value
            if (parameterKey.DefaultValueMetadata != null)
            {
                fixed (byte* dataValues = DataValues)
                    parameterKey.DefaultValueMetadata.WriteBuffer((IntPtr)dataValues + memberOffset, 16);
            }

            return new Accessor(memberOffset, elementCount);
        }

        /// <summary>
        ///   Sets the value of a parameter to an object.
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <param name="parameter">Parameter to set.</param>
        /// <param name="value">Value of the object.</param>
        public void Set<T>(ObjectParameterKey<T> parameter, T value)
        {
            Set(GetAccessor(parameter), value);
        }

        /// <summary>
        ///   Gets the value of a parameter as an object.
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <param name="parameter">Parameter to get.</param>
        /// <returns>Value of the object.</returns>
        public T Get<T>(ObjectParameterKey<T> parameter, bool createIfNew = false)
        {
            var accessor = GetAccessor(parameter, createIfNew);
            if (accessor.BindingSlot == -1)
                return parameter.DefaultValueMetadataT.DefaultValue;

            return Get(accessor);
        }

        /// <summary>
        ///   Sets the value of a permutation parameter.
        /// </summary>
        /// <typeparam name="T">Type of the permutation parameter.</typeparam>
        /// <param name="parameter">Parameter to set.</param>
        /// <param name="value">Value of the permutation parameter.</param>
        public void Set<T>(PermutationParameterKey<T> parameter, T value)
        {
            Set(GetAccessor(parameter), value);
        }

        /// <summary>
        ///   Gets the value of a permutation parameter.
        /// </summary>
        /// <typeparam name="T">Type of the permutation parameter.</typeparam>
        /// <param name="parameter">Parameter to get.</param>
        /// <returns>Value of the permutation parameter.</returns>
        public T Get<T>(PermutationParameterKey<T> parameter, bool createIfNew = false)
        {
            var accessor = GetAccessor(parameter, createIfNew);
            if (accessor.BindingSlot == -1)
                return parameter.DefaultValueMetadataT.DefaultValue;

            return Get(accessor);
        }

        /// <summary>
        ///   Sets the value of a parameter.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="parameter">Parameter to set.</param>
        /// <param name="value">Value of the parameter.</param>
        public void Set<T>(ValueParameterKey<T> parameter, T value) where T : struct
        {
            Set(GetAccessor(parameter), ref value);
        }

        /// <summary>
        ///   Sets the value of a parameter.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="parameter">Parameter to set.</param>
        /// <param name="value">Value of the parameter.</param>
        public void Set<T>(ValueParameterKey<T> parameter, ref T value) where T : struct
        {
            Set(GetAccessor(parameter), ref value);
        }

        /// <summary>
        ///   Sets the values of a parameter.
        /// </summary>
        /// <typeparam name="T">Type of the values.</typeparam>
        /// <param name="parameter">Parameter to set.</param>
        /// <param name="values">Values of the parameter.</param>
        public void Set<T>(ValueParameterKey<T> parameter, T[] values) where T : struct
        {
            Set(GetAccessor(parameter, values.Length), values.Length, ref values[0]);
        }

        /// <summary>
        ///   Sets the values of a parameter.
        /// </summary>
        /// <typeparam name="T">Type of the values.</typeparam>
        /// <param name="parameter">Parameter to set.</param>
        /// <param name="count">Number of parameters to set.</param>
        /// <param name="firstValue">Reference to the first of the values of the parameter.</param>
        public void Set<T>(ValueParameterKey<T> parameter, int count, ref T firstValue) where T : struct
        {
            Set(GetAccessor(parameter, count), count, ref firstValue);
        }

        /// <summary>
        ///   Gets the value of a parameter.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="parameter">Parameter to read.</param>
        /// <returns>Value of the parameter.</returns>
        public T Get<T>(ValueParameterKey<T> parameter) where T : struct
        {
            return Get(GetAccessor(parameter));
        }

        /// <summary>
        ///   Gets the values of a parameter.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="parameter">Parameter to read.</param>
        /// <returns>Values of the parameter as an array.</returns>
        public unsafe T[] GetValues<T>(ValueParameterKey<T> parameter) where T : struct
        {
            var accesor = GetAccessor(parameter);

            // Align to float4
            var stride = (Utilities.SizeOf<T>() + 15) / 16 * 16;
            var values = new T[accesor.Count];

            fixed (byte* dataValues = DataValues)
            {
                var dataPtr = (IntPtr)dataValues + accesor.Offset;

                for (int i = 0; i < values.Length; ++i)
                {
                    Utilities.Read(dataPtr, ref values[i]);
                    dataPtr += stride;
                }

                return values;
            }
        }

        /// <summary>
        ///   Copies all values of a given parameter to another <see cref="ParameterCollection"/>.
        /// </summary>
        /// <typeparam name="T">Type of the values.</typeparam>
        /// <param name="parameter">The parameter whose values are going to be copied.</param>
        /// <param name="destination">The collection to copy the values to.</param>
        /// <param name="destinationKey">The parameter key for the values of the destination collection.</param>
        /// <exception cref="IndexOutOfRangeException"><paramref name="parameter"/> has more values than <paramref name="destinationKey"/>.</exception>
        public unsafe void CopyTo<T>(ValueParameterKey<T> parameter, ParameterCollection destination, ValueParameterKey<T> destinationKey) where T : struct
        {
            var sourceParameter = GetAccessor(parameter);
            var destParameter = destination.GetAccessor(destinationKey, sourceParameter.Count);
            if (sourceParameter.Count > destParameter.Count)
                throw new IndexOutOfRangeException();

            // Align to float4
            var stride = (Utilities.SizeOf<T>() + 15) / 16 * 16;
            var sizeInBytes = sourceParameter.Count * stride;

            fixed (byte* sourceDataValues = DataValues)
            fixed (byte* destDataValues = destination.DataValues)
            {
                var sourcePtr = (IntPtr) sourceDataValues + sourceParameter.Offset;
                var destPtr = (IntPtr) destDataValues + destParameter.Offset;
                Utilities.CopyMemory(destPtr, sourcePtr, sizeInBytes);
            }
        }

        /// <summary>
        ///   Sets the value of a parameter.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="parameter">Parameter whose value is going to be set.</param>
        /// <param name="value">New value for the parameter.</param>
        public unsafe void Set<T>(ValueParameter<T> parameter, T value) where T : struct
        {
            fixed (byte* dataValues = DataValues)
                Utilities.Write((IntPtr) dataValues + parameter.Offset, ref value);
        }

        /// <summary>
        ///   Sets the value of a parameter.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="parameter">Parameter whose value is going to be set.</param>
        /// <param name="value">By reference. New value for the parameter.</param>
        public unsafe void Set<T>(ValueParameter<T> parameter, ref T value) where T : struct
        {
            fixed (byte* dataValues = DataValues)
                Utilities.Write((IntPtr) dataValues + parameter.Offset, ref value);
        }

        /// <summary>
        ///   Sets the values of a parameter.
        /// </summary>
        /// <typeparam name="T">Type of the values.</typeparam>
        /// <param name="parameter">Parameter whose values are going to be set.</param>
        /// <param name="count">Number of values to set.</param>
        /// <param name="firstValue">Reference of the first value to set.</param>
        /// <exception cref="IndexOutOfRangeException"><paramref name="count"/> is greater than the number of values in <paramref name="parameter"/>.</exception>
        public unsafe void Set<T>(ValueParameter<T> parameter, int count, ref T firstValue) where T : struct
        {
            // Align to float4
            var stride = (Utilities.SizeOf<T>() + 15) / 16 * 16;
            var elementCount = parameter.Count;
            if (count > elementCount)
                throw new IndexOutOfRangeException();

            fixed (byte* dataValues = DataValues)
            {
                var dataPtr = (IntPtr) dataValues + parameter.Offset;

                var value = Interop.Pin(ref firstValue);
                for (int i = 0; i < count; ++i)
                {
                    Utilities.Write(dataPtr, ref value);
                    dataPtr += stride;

                    value = Interop.IncrementPinned(value);
                }
            }
        }

        /// <summary>
        ///   Sets a permutation.
        /// </summary>
        /// <typeparam name="T">Type of the value of the permutation parameter.</typeparam>
        /// <param name="parameter">Permutation parameter to set.</param>
        /// <param name="value">Value for the permutation parameter.</param>
        public void Set<T>(PermutationParameter<T> parameter, T value)
        {
            bool isSame = EqualityComparer<T>.Default.Equals((T) ObjectValues[parameter.BindingSlot], value);
            if (!isSame)
                PermutationCounter++;

            // For value types, we don't assign again because this causes boxing
            if (!typeof(T).IsValueType || !isSame)
            {
                ObjectValues[parameter.BindingSlot] = value;
            }
        }

        /// <summary>
        ///   Sets an object.
        /// </summary>
        /// <typeparam name="T">Type of the object to set.</typeparam>
        /// <param name="parameterAccessor">Accesor to the parameter to set.</param>
        /// <param name="value">Object to set.</param>
        public void Set<T>(ObjectParameterAccessor<T> parameterAccessor, T value)
        {
            ObjectValues[parameterAccessor.BindingSlot] = value;
        }

        /// <summary>
        ///   Gets a value.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="parameter">Parameter to get.</param>
        /// <returns>Value of the parameter.</returns>
        public unsafe T Get<T>(ValueParameter<T> parameter) where T : struct
        {
            fixed (byte* dataValues = DataValues)
                return Utilities.Read<T>((IntPtr) dataValues + parameter.Offset);
        }

        /// <summary>
        ///   Gets a permutation.
        /// </summary>
        /// <typeparam name="T">Type of the value of the permutation parameter.</typeparam>
        /// <param name="parameter">Permutation parameter to get.</param>
        /// <returns>Value of the permutation parameter.</returns>
        public T Get<T>(PermutationParameter<T> parameter)
        {
            return (T) ObjectValues[parameter.BindingSlot];
        }

        /// <summary>
        ///   Gets an object.
        /// </summary>
        /// <typeparam name="T">Type of object to get.</typeparam>
        /// <param name="parameterAccessor">Accesor to the parameter to get.</param>
        /// <returns>Object set in the parameter.</returns>
        public T Get<T>(ObjectParameterAccessor<T> parameterAccessor)
        {
            return (T) ObjectValues[parameterAccessor.BindingSlot];
        }

        public void SetObject(ParameterKey key, object value)
        {
            if (key.Type != ParameterKeyType.Permutation &&
                key.Type != ParameterKeyType.Object)
                throw new InvalidOperationException("SetObject can only be used for Permutation or Object keys.");

            var accessor = GetObjectParameterHelper(key);

            if (key.Type == ParameterKeyType.Permutation)
            {
                var oldValue = ObjectValues[accessor.Offset];
                if ((oldValue != null && (value is null || !oldValue.Equals(value))) || // oldValue non-null => check equality
                    (oldValue is null && value != null))                                // oldValue null => check if value too
                        PermutationCounter++;
            }
            ObjectValues[accessor.Offset] = value;
        }

        public object GetObject(ParameterKey key)
        {
            if (key.Type != ParameterKeyType.Permutation &&
                key.Type != ParameterKeyType.Object)
                throw new InvalidOperationException("GetObject can only be used for Permutation or Object keys.");

            var accessor = GetObjectParameterHelper(key, createIfNew: false);
            if (accessor.Offset == -1)
                return null;

            return ObjectValues[accessor.Offset];
        }

        public bool Remove(ParameterKey key)
        {
            for (int i = 0; i < parameterKeyInfos.Count; ++i)
            {
                if (parameterKeyInfos[i].Key == key)
                {
                    parameterKeyInfos.SwapRemoveAt(i);
                    LayoutCounter++;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///   Clears the collection, including the layout.
        /// </summary>
        public void Clear()
        {
            DataValues = EmptyData;
            ObjectValues = null;
            layout = null;
            parameterKeyInfos.Clear();
        }

        /// <summary>
        ///   Determines whether the collection contains a value.
        /// </summary>
        /// <param name="key">Parameter key of the value to look for.</param>
        /// <returns><c>true</c> if the collection contains the value; otherwise, <c>false</c>.</returns>
        public bool ContainsKey(ParameterKey key)
        {
            for (int i = 0; i < parameterKeyInfos.Count; ++i)
                if (parameterKeyInfos[i].Key == key)
                    return true;

            return false;
        }

        /// <summary>
        ///   Reorganizes the internal data and resources to match the given objects, and appends extra values at the end.
        /// </summary>
        /// <param name="collectionLayout">Layout of the parameter collection.</param>
        public unsafe void UpdateLayout(ParameterCollectionLayout collectionLayout)
        {
            var oldLayout = layout;
            layout = collectionLayout;

            // Same layout, or removed layout
            if (oldLayout == collectionLayout || collectionLayout is null)
                return;

            var layoutParameterKeyInfos = collectionLayout.LayoutParameterKeyInfos;

            // Do a first pass to measure constant buffer size
            var newParameterKeyInfos = new FastList<ParameterKeyInfo>(Math.Max(1, parameterKeyInfos.Count));
            newParameterKeyInfos.AddRange(parameterKeyInfos);
            var processedParameters = new bool[parameterKeyInfos.Count];

            var bufferSize = collectionLayout.BufferSize;
            var resourceCount = collectionLayout.ResourceCount;

            foreach (var layoutParameterKeyInfo in layoutParameterKeyInfos)
            {
                // Find the same parameter in old collection
                // Is this parameter already added?
                for (int i = 0; i < parameterKeyInfos.Count; ++i)
                {
                    if (parameterKeyInfos[i].Key == layoutParameterKeyInfo.Key)
                    {
                        processedParameters[i] = true;
                        newParameterKeyInfos.Items[i] = layoutParameterKeyInfo;
                        break;
                    }
                }
            }

            // Append new elements that don't exist in new layouts (to preserve their values)
            for (int i = 0; i < processedParameters.Length; ++i)
            {
                // Skip parameters already processed before
                if (processedParameters[i])
                    continue;

                var parameterKeyInfo = newParameterKeyInfos[i];

                if (parameterKeyInfo.Offset != -1)
                {
                    // It's data
                    newParameterKeyInfos.Items[i].Offset = bufferSize;

                    var elementSize = newParameterKeyInfos.Items[i].Key.Size;
                    var elementCount = newParameterKeyInfos.Items[i].Count;
                    var totalSize = elementSize + (elementSize + 15) / 16 * 16 * (elementCount - 1);
                    bufferSize += totalSize;
                }
                else if (parameterKeyInfo.BindingSlot != -1)
                {
                    // It's a resource
                    newParameterKeyInfos.Items[i].BindingSlot = resourceCount++;
                }
            }

            var newDataValues = new byte[bufferSize];
            var newResourceValues = new object[resourceCount];

            // Update default values
            var bufferOffset = 0;
            foreach (var layoutParameterKeyInfo in layoutParameterKeyInfos)
            {
                if (layoutParameterKeyInfo.Offset != -1)
                {
                    // It's data
                    // TODO: Set default value
                    var defaultValueMetadata = layoutParameterKeyInfo.Key?.DefaultValueMetadata;
                    if (defaultValueMetadata != null)
                    {
                        fixed (byte* newDataValuesPtr = newDataValues)
                            defaultValueMetadata.WriteBuffer((IntPtr) newDataValuesPtr + bufferOffset + layoutParameterKeyInfo.Offset, 16);
                    }
                }
            }

            // Second pass to copy existing data at new offsets/slots
            for (int i = 0; i < parameterKeyInfos.Count; ++i)
            {
                var parameterKeyInfo = parameterKeyInfos[i];
                var newParameterKeyInfo = newParameterKeyInfos[i];

                if (newParameterKeyInfo.Offset != -1)
                {
                    var elementSize = newParameterKeyInfos.Items[i].Key.Size;
                    var newTotalSize = elementSize + (elementSize + 15) / 16 * 16 * (newParameterKeyInfo.Count - 1);
                    var totalSize = elementSize + (elementSize + 15) / 16 * 16 * (parameterKeyInfo.Count - 1);

                    // It's data
                    fixed (byte* dataValues = DataValues)
                    fixed (byte* newDataValuesPtr = newDataValues)
                        Utilities.CopyMemory((IntPtr) newDataValuesPtr + newParameterKeyInfo.Offset, (IntPtr)dataValues + parameterKeyInfo.Offset, Math.Min(newTotalSize, totalSize));
                }
                else if (newParameterKeyInfo.BindingSlot != -1)
                {
                    // It's a resource
                    newResourceValues[newParameterKeyInfo.BindingSlot] = ObjectValues[parameterKeyInfo.BindingSlot];
                }
            }

            // Update new content
            parameterKeyInfos = newParameterKeyInfos;

            DataValues = newDataValues;
            ObjectValues = newResourceValues;
        }

        protected Accessor GetObjectParameterHelper(ParameterKey parameterKey, bool createIfNew = true)
        {
            // Find existing first
            for (int i = 0; i < parameterKeyInfos.Count; ++i)
            {
                if (parameterKeyInfos.Items[i].Key == parameterKey)
                {
                    return parameterKeyInfos.Items[i].GetObjectAccessor();
                }
            }

            if (!createIfNew)
                return new Accessor(-1, 0);

            if (parameterKey.Type == ParameterKeyType.Permutation)
                PermutationCounter++;

            LayoutCounter++;

            // Check layout if it exists
            if (layout != null)
            {
                foreach (var layoutParameterKeyInfo in layout.LayoutParameterKeyInfos)
                {
                    if (layoutParameterKeyInfo.Key == parameterKey)
                    {
                        parameterKeyInfos.Add(layoutParameterKeyInfo);
                        return layoutParameterKeyInfo.GetObjectAccessor();
                    }
                }
            }

            // Create info entry
            var resourceValuesSize = ObjectValues?.Length ?? 0;
            Array.Resize(ref ObjectValues, resourceValuesSize + 1);
            parameterKeyInfos.Add(new ParameterKeyInfo(parameterKey, resourceValuesSize));

            // Initialize default value
            if (parameterKey.DefaultValueMetadata != null)
            {
                ObjectValues[resourceValuesSize] = parameterKey.DefaultValueMetadata.GetDefaultValue();
            }

            return new Accessor(resourceValuesSize, count: 1);
        }

        public class Serializer : ClassDataSerializer<ParameterCollection>
        {
            public override void Serialize(ref ParameterCollection parameterCollection, ArchiveMode mode, SerializationStream stream)
            {
                stream.Serialize(ref parameterCollection.parameterKeyInfos, mode);
                stream.SerializeExtended(ref parameterCollection.ObjectValues, mode);
                stream.Serialize(ref parameterCollection.DataValues, mode);
            }
        }

        public struct Copier
        {
            private CopyRange[] ranges;
            private readonly ParameterCollection destination;
            private readonly ParameterCollection source;
            private int sourceLayoutCounter;
            private readonly string subKey;

            public Copier(ParameterCollection destination, ParameterCollection source, string subKey = null)
            {
                ranges = null;
                sourceLayoutCounter = 0;
                this.destination = destination;
                this.source = source;
                this.subKey = subKey;
            }

            public unsafe void Copy()
            {
                var destinationLayout = destination.Layout;

                // NOTE: We should provide a slow version for first copy during Extract (layout isn't known yet)
                if (destinationLayout is null)
                    throw new NotImplementedException();

                if (destinationLayout == source.Layout)
                {
                    // Easy, let's do a full copy!
                    PerformFastCopy(destinationLayout);
                }
                else
                {
                    if (ranges is null || sourceLayoutCounter != source.LayoutCounter)
                    {
                        Compile();

                        // Try again in case fast copy is possible
                        if (destinationLayout == source.Layout)
                        {
                            PerformFastCopy(destinationLayout);
                            return;
                        }
                    }

                    // Slower path: copy element by element
                    foreach (var range in ranges)
                    {
                        if (range.IsResource)
                        {
                            for (int i = 0; i < range.Size; ++i)
                            {
                                destination.ObjectValues[range.DestStart + i] = source.ObjectValues[range.SourceStart + i];
                            }
                        }
                        else if (range.IsData)
                        {
                            fixed (byte* destDataValues = destination.DataValues)
                            fixed (byte* sourceDataValues = source.DataValues)
                                Utilities.CopyMemory((IntPtr) destDataValues + range.DestStart, (IntPtr) sourceDataValues + range.SourceStart, range.Size);
                        }
                    }
                }
            }

            private unsafe void PerformFastCopy(ParameterCollectionLayout destinationLayout)
            {
                fixed (byte* destPtr = destination.DataValues)
                fixed (byte* sourcePtr = source.DataValues)
                    Utilities.CopyMemory((IntPtr) destPtr, (IntPtr) sourcePtr, destinationLayout.BufferSize);

                var resourceCount = destinationLayout.ResourceCount;
                for (int i = 0; i < resourceCount; ++i)
                    destination.ObjectValues[i] = source.ObjectValues[i];
            }

            private void Compile()
            {
                // If we are first, let's apply our layout!
                if (source.Layout is null && subKey is null)
                {
                    source.UpdateLayout(destination.Layout);
                    return;
                }
                else
                {
                    // TODO: GRAPHICS REFACTOR optim: check if layout is the same
                    //if (source.Layout.LayoutParameterKeyInfos == destination.Layout.LayoutParameterKeyInfos)
                }

                var rangesList = new List<CopyRange>();

                // Try to match elements (both source and destination should have a layout by now)
                foreach (var parameterKeyInfo in destination.Layout.LayoutParameterKeyInfos)
                {
                    var sourceKey = parameterKeyInfo.Key;
                    if (subKey != null && sourceKey.Name.EndsWith(subKey))
                    {
                        // That's a match
                        var subkeyName = parameterKeyInfo.Key.Name.Substring(0, parameterKeyInfo.Key.Name.Length - subKey.Length);
                        sourceKey = ParameterKeys.FindByName(subkeyName);
                    }

                    if (parameterKeyInfo.Key.Type == ParameterKeyType.Value)
                    {
                        var sourceAccessor = source.GetValueAccessorHelper(sourceKey, parameterKeyInfo.Count);
                        var destAccessor = destination.GetValueAccessorHelper(parameterKeyInfo.Key, parameterKeyInfo.Count);
                        var elementCount = Math.Min(sourceAccessor.Count, destAccessor.Count);
                        var elementSize = parameterKeyInfo.Key.Size;
                        var size = (elementSize + 15) / 16 * 16 * (elementCount - 1) + elementSize;
                        rangesList.Add(new CopyRange { IsData = true, SourceStart = sourceAccessor.Offset, DestStart = destAccessor.Offset, Size = size });
                    }
                    else
                    {
                        var sourceAccessor = source.GetObjectParameterHelper(sourceKey);
                        var destAccessor = destination.GetObjectParameterHelper(parameterKeyInfo.Key);
                        var elementCount = Math.Min(sourceAccessor.Count, destAccessor.Count);
                        rangesList.Add(new CopyRange { IsResource = true, SourceStart = sourceAccessor.Offset, DestStart = destAccessor.Offset, Size = elementCount });
                    }
                }

                ranges = rangesList.ToArray();
                sourceLayoutCounter = source.LayoutCounter;
            }
        }

        public struct CompositionCopier
        {
            private List<CopyRange> ranges;
            private ParameterCollection destination;

            public bool IsValid => ranges != null;

            /// <summary>
            /// Copies data from source to destination according to previously compiled layout.
            /// </summary>
            /// <param name="source"></param>
            public unsafe void Copy(ParameterCollection source)
            {
                foreach (var range in ranges)
                {
                    if (range.IsResource)
                    {
                        for (int i = 0; i < range.Size; ++i)
                        {
                            destination.ObjectValues[range.DestStart + i] = source.ObjectValues[range.SourceStart + i];
                        }
                    }
                    else if (range.IsData)
                    {
                        fixed (byte* destDataValues = destination.DataValues)
                        fixed (byte* sourceDataValues = source.DataValues)
                        {
                            uint* destPtr = (uint*)(destDataValues + range.DestStart);
                            uint* sourcePtr = (uint*)(sourceDataValues + range.SourceStart);
                            var count = range.Size / 4;
                            for (int i = 0; i < count; ++i)
                                *destPtr++ = *sourcePtr++;
                        }
                    }
                }
            }

            /// <summary>
            ///   Computes the copy operations. Assumes destination layout is sequential.
            /// </summary>
            /// <param name="dest">Destination parameter collection where to copy to.</param>
            /// <param name="source">Source parameter collection where to copy from.</param>
            /// <param name="keyRoot"></param>
            public void Compile(ParameterCollection dest, ParameterCollection source, string keyRoot)
            {
                ranges = new List<CopyRange>();
                destination = dest;
                var sourceLayout = new ParameterCollectionLayout();

                // Helper structures to try to keep range contiguous and have as few copy operations as possible (note: there can be some padding)
                var currentDataRange = new CopyRange { IsData = true, DestStart = -1 };
                var currentResourceRange = new CopyRange { IsResource = true, DestStart = -1 };

                // Iterate over each variable in dest, and if they match keyRoot, create the equivalent layout in source
                foreach (var parameterKeyInfo in dest.Layout.LayoutParameterKeyInfos)
                {
                    bool isResource = parameterKeyInfo.BindingSlot != -1;
                    bool isData = parameterKeyInfo.Offset != -1;

                    if (parameterKeyInfo.Key.Name.EndsWith(keyRoot))
                    {
                        // That's a match
                        var subkeyName = parameterKeyInfo.Key.Name.Substring(0, parameterKeyInfo.Key.Name.Length - keyRoot.Length);
                        var subkey = ParameterKeys.FindByName(subkeyName);

                        if (isData)
                        {
                            // First time since range reset, let's setup destination offset
                            if (currentDataRange.DestStart == -1)
                                currentDataRange.DestStart = parameterKeyInfo.Offset;

                            // Might be some empty space (padding)
                            currentDataRange.Size = parameterKeyInfo.Offset - currentDataRange.DestStart;

                            sourceLayout.LayoutParameterKeyInfos.Add(new ParameterKeyInfo(subkey, currentDataRange.SourceStart + currentDataRange.Size, parameterKeyInfo.Count));

                            var elementCount = parameterKeyInfo.Count;
                            var elementSize = parameterKeyInfo.Key.Size;
                            var size = (elementSize + 15) / 16 * 16 * (elementCount - 1) + elementSize;

                            currentDataRange.Size += size;
                        }
                        else if (isResource)
                        {
                            // First time since range reset, let's setup destination offset
                            if (currentResourceRange.DestStart == -1)
                                currentResourceRange.DestStart = parameterKeyInfo.BindingSlot;

                            // Might be some empty space (padding) (probably unlikely for resources...)
                            currentResourceRange.Size = parameterKeyInfo.BindingSlot - currentResourceRange.DestStart;

                            sourceLayout.LayoutParameterKeyInfos.Add(new ParameterKeyInfo(subkey, currentResourceRange.SourceStart + currentResourceRange.Size));

                            currentResourceRange.Size += parameterKeyInfo.Count;
                        }
                    }
                    else
                    {
                        // Found one item not part of the range, let's finish it
                        if (isData)
                            FlushRangeIfNecessary(ref currentDataRange);
                        else if (isResource)
                            FlushRangeIfNecessary(ref currentResourceRange);
                    }
                }

                // Finish ranges
                FlushRangeIfNecessary(ref currentDataRange);
                FlushRangeIfNecessary(ref currentResourceRange);

                // Update sizes
                sourceLayout.BufferSize = currentDataRange.SourceStart;
                sourceLayout.ResourceCount = currentResourceRange.SourceStart;

                source.UpdateLayout(sourceLayout);
            }

            private void FlushRangeIfNecessary(ref CopyRange currentRange)
            {
                if (currentRange.Size > 0)
                {
                    ranges.Add(currentRange);
                    currentRange.SourceStart += currentRange.Size;
                    currentRange.DestStart = -1;
                    currentRange.Size = 0;
                }
            }
        }

        private struct CopyRange
        {
            public bool IsResource;
            public bool IsData;
            public int SourceStart;
            public int DestStart;
            public int Size;
        }

        public struct Accessor
        {
            public int Offset;
            public int Count;

            internal Accessor(int offset, int count)
            {
                Offset = offset;
                Count = count;
            }
        }

        private class DebugView
        {
            private readonly ParameterCollection collection;

            public DebugView(ParameterCollection collection)
            {
                this.collection = collection;
            }

            public ParameterCollectionLayout Layout => collection.Layout;

            public int PermutationCounter => collection.PermutationCounter;

            // NOTE: This should be named "Parameters", but since its name is hidden and we want to appear after PermutationCounter, we prepend ZZ
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public unsafe object[] ZZParameters
            {
                get
                {
                    return collection.ParameterKeyInfos
                        .OrderBy(x => x.Key.Type == ParameterKeyType.Value ? 0x10000 + x.Offset : x.BindingSlot) // sort by offsets or binding slot (note: values will go after objects)
                        .SelectMany(x =>
                        {
                            if (x.Key.Type == ParameterKeyType.Value)
                            {
                                // Values
                                var stride = (x.Key.Size + 15) / 16 * 16;
                                var values = new object[x.Count];
                                fixed (byte* dataValuesStart = collection.DataValues)
                                {
                                    var offset = x.Offset;
                                    for (int i = 0; i < x.Count; ++i, offset += stride)
                                    {
                                        // Safety check: Check if we read outside of array
                                        var outOfBound = offset + x.Key.Size > collection.DataValues.Length;

                                        // Create debug object for this parameter
                                        values[i] = new ValueParameter
                                        {
                                            Key = x.Key,
                                            Index = x.Count > 1 ? i : -1,
                                            Offset = offset,
                                            Value = outOfBound ? "Error (out of bound)" : x.Key.ReadValue((IntPtr)dataValuesStart + offset),
                                        };
                                    }
                                }
                                return values;
                            }
                            else
                            {
                                // Objects and permutations
                                var objects = new object[x.Count];
                                var slot = x.BindingSlot;
                                for (int i = 0; i < x.Count; ++i, ++slot)
                                {
                                    // Create debug object for this parameter
                                    objects[i] = new ObjectParameter
                                    {
                                        Key = x.Key,
                                        Index = x.Count > 1 ? i : -1,
                                        BindingSlot = slot,
                                        Value = collection.ObjectValues[slot],
                                    };
                                }
                                return objects;
                            }
                        })
                        .ToArray();
                }
            }

            // Represents a value
            private class ValueParameter
            {
                public ParameterKey Key;
                public object Value;
                public int Index;
                public int Offset;

                public override string ToString()
                {
                    return $"{Key.Type} @ Offset {Offset:X4}: {Key}{(Index != -1 ? "[" + Index + "]" : string.Empty)} => {Value ?? "null"}";
                }
            }

            // Represents an object or permutation
            private class ObjectParameter
            {
                public ParameterKey Key;
                public object Value;
                public int Index;
                public int BindingSlot;

                public override string ToString()
                {
                    return $"{Key.Type} @ Slot {BindingSlot:D2}: {Key}{(Index != -1 ? "[" + Index + "]" : string.Empty)} => {Value ?? "null"}";
                }
            }
        }
    }
}
