// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Reflection;
using System.Runtime.CompilerServices;

#pragma warning disable 436 // Xenko.PublicKeys is defined in multiple assemblies

[assembly: InternalsVisibleTo("Xenko.Engine" + Xenko.PublicKeys.Default)]
