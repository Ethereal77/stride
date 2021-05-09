// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Assets
{
    /// <summary>
    /// A delegate used for events raised when the dirty flag of an object has changed
    /// </summary>
    /// <param name="sender">The object that had its dirty flag changed.</param>
    /// <param name="oldValue">The old value of the dirty flag.</param>
    /// <param name="newValue">The new value of the dirty flag.</param>
    public delegate void DirtyFlagChangedDelegate<in T>(T sender, bool oldValue, bool newValue);
}
