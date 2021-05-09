// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Assets.Scripts
{
    /// <summary>
    ///   Defines the accessibility kinds of a <see cref="VisualScriptAsset"/>, <see cref="Method"/> or <see cref="Symbol"/>.
    /// </summary>
    public enum Accessibility
    {
        Public = 0,
        Private = 1,
        Protected = 2,
        Internal = 3,
        ProtectedOrInternal = 4
    }
}
