// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

#pragma warning disable SA1649 // File name must match first type name

using System;

namespace Stride.Updater
{
    /// <summary>
    /// Defines how to set and get values from a property of a given value type for the <see cref="UpdateEngine"/>.
    /// </summary>
    /// <typeparam name="T">The property type.</typeparam>
    public class UpdatableProperty<T> : UpdatableProperty where T : struct
    {
        public UpdatableProperty(IntPtr getter, bool virtualDispatchGetter, IntPtr setter, bool virtualDispatchSetter) : base(getter, virtualDispatchGetter, setter, virtualDispatchSetter)
        {
        }

        /// <inheritdoc/>
        public override Type MemberType
        {
            get { return typeof(T); }
        }

        /// <inheritdoc/>
        public override IntPtr GetStructAndUnbox(IntPtr obj, object data)
        {
#if IL
            ldarg data
            // TEMP XAMARIN AOT FIX -- not sure why we can't use inline directly here
            // unbox !T
            call native int Stride.Updater.UpdateEngineHelper::Unbox<!T>(object)
            dup
            ldarg obj
            ldarg.0
            ldfld native int class Stride.Updater.UpdatableProperty::Getter
            calli instance !T()
            stobj !T
            ret
#endif
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void GetBlittable(IntPtr obj, IntPtr data)
        {
#if IL
            ldarg data
            ldarg obj
            ldarg.0
            ldfld native int class Stride.Updater.UpdatableProperty::Getter
            calli instance !T()
            stobj !T
            ret
#endif
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void SetStruct(IntPtr obj, object data)
        {
#if IL
            ldarg obj
            ldarg data
            unbox.any !T
            ldarg.0
            ldfld native int class Stride.Updater.UpdatableProperty::Setter
            calli instance void(!T)
            ret
#endif
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void SetBlittable(IntPtr obj, IntPtr data)
        {
#if IL
            ldarg obj
            ldarg data
            ldobj !T
            ldarg.0
            ldfld native int class Stride.Updater.UpdatableProperty::Setter
            calli instance void(!T)
            ret
#endif
            throw new NotImplementedException();
        }
    }
}
