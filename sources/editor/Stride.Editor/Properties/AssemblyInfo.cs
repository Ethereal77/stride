// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("361bd08b-e0a3-44f3-839a-d10a7089f7d0")]

#pragma warning disable CS0436

[assembly: InternalsVisibleTo("Stride.GameStudio" + Stride.PublicKeys.Default)]

[assembly: ThemeInfo(
    // Where theme specific resource dictionaries are located
    // (used if a resource is not found in the page or application resource dictionaries)
    ResourceDictionaryLocation.None,
    // Where the generic resource dictionary is located
    // (used if a resource is not found in the page, app, or any theme specific resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly)]
