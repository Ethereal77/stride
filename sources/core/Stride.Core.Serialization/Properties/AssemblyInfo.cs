// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Reflection;
using System.Runtime.CompilerServices;

#pragma warning disable 436 // Xenko.PublicKeys is defined in multiple assemblies

// Make internals Xenko.Framework.visible to all Xenko.Framework.assemblies
[assembly: InternalsVisibleTo("Xenko.Core.Serialization.Serializers" + Xenko.PublicKeys.Default)]
