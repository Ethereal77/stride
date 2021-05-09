// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Runtime.CompilerServices;

using Stride.Core.Reflection;
using Stride.Core.Translation;
using Stride.Core.Translation.Providers;
using Stride.Core.Assets.Quantum;
using Stride.Assets.Entities;
using Stride.Assets.SpriteFont;
using Stride.Assets.Presentation.Templates;
using Stride.Rendering;
using Stride.Rendering.Materials;

namespace Stride.Assets.Presentation
{
    internal class Module
    {
        [Core.ModuleInitializer]
        public static void Initialize()
        {
            RuntimeHelpers.RunModuleConstructor(typeof(SpriteFontAsset).Module.ModuleHandle);
            RuntimeHelpers.RunModuleConstructor(typeof(MaterialKeys).Module.ModuleHandle);
            RuntimeHelpers.RunModuleConstructor(typeof(Model).Module.ModuleHandle);
            RuntimeHelpers.RunModuleConstructor(typeof(PrefabAsset).Module.ModuleHandle);

            var thisAssembly = typeof(Module).Assembly;
            AssemblyRegistry.Register(thisAssembly, AssemblyCommonCategories.Assets);

            // We need access to the AssetQuantumRegistry from the SessionTemplateGenerator.
            // For now we register graph types in the module initializer
            AssetQuantumRegistry.RegisterAssembly(thisAssembly);

            // Register the default templates
            StrideTemplates.Register();

            // Initialize translation
            TranslationManager.Instance.RegisterProvider(new GettextTranslationProvider());
        }
    }
}
