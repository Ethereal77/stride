// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Runtime.CompilerServices;

#pragma warning disable 436 // Stride.PublicKeys is defined in multiple assemblies

// Make internals Stride visible to Stride assemblies
[assembly: InternalsVisibleTo("Stride.Engine" + Stride.PublicKeys.Default)]
[assembly: InternalsVisibleTo("Stride.Rendering " + Stride.PublicKeys.Default)]
[assembly: InternalsVisibleTo("Stride.Graphics" + Stride.PublicKeys.Default)]
[assembly: InternalsVisibleTo("Stride.Audio" + Stride.PublicKeys.Default)]
[assembly: InternalsVisibleTo("Stride.Games" + Stride.PublicKeys.Default)]
[assembly: InternalsVisibleTo("Stride.Graphics.Regression" + Stride.PublicKeys.Default)]
