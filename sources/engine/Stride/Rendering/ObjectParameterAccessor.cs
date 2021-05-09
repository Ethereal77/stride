// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Rendering
{
    /// <summary>
    ///   Represents a structure that defines a direct way of accesing an object parameter in an effect.
    /// </summary>
    /// <typeparam name="T">Type of the object.</typeparam>
    public struct ObjectParameterAccessor<T>
    {
        internal readonly int BindingSlot;
        internal readonly int Count;

        internal ObjectParameterAccessor(int bindingSlot, int count)
        {
            BindingSlot = bindingSlot;
            Count = count;
        }
    }
}
