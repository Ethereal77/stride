// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Runtime.CompilerServices;

// Make internals Stride.Framework.visible to all Stride.Framework.assemblies
[assembly: InternalsVisibleTo("Stride.Core.IO" + Stride.PublicKeys.Default)]
[assembly: InternalsVisibleTo("Stride.Core.Assets" + Stride.PublicKeys.Default)]
[assembly: InternalsVisibleTo("Stride" + Stride.PublicKeys.Default)]
[assembly: InternalsVisibleTo("Stride.UI" + Stride.PublicKeys.Default)]
[assembly: InternalsVisibleTo("Stride.Engine" + Stride.PublicKeys.Default)]
[assembly: InternalsVisibleTo("Stride.Rendering" + Stride.PublicKeys.Default)]
[assembly: InternalsVisibleTo("Stride.Graphics" + Stride.PublicKeys.Default)]
[assembly: InternalsVisibleTo("Stride.Games" + Stride.PublicKeys.Default)]
[assembly: InternalsVisibleTo("Stride.Audio" + Stride.PublicKeys.Default)]
[assembly: InternalsVisibleTo("Stride.Video" + Stride.PublicKeys.Default)]
[assembly: InternalsVisibleTo("Stride.Core.Tests" + Stride.PublicKeys.Default)]
