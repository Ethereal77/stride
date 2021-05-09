// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Stride.Core.Annotations;
using Stride.Core.Extensions;
using Stride.Core.Diagnostics;
using Stride.Core.Presentation.View;
using Stride.Core.Assets.Editor.Components.Properties;
using Stride.Core.Assets.Editor.ViewModel;

namespace Stride.Core.Assets.Editor.Services
{
    public abstract class AssetsPlugin
    {
        internal static readonly List<AssetsPlugin> RegisteredPlugins = new List<AssetsPlugin>();

        // TODO: Give access to this differently
        public readonly List<PackageSettingsEntry> ProfileSettings = new List<PackageSettingsEntry>();

        public static IReadOnlyDictionary<Type, object> TypeImagesDictionary => TypeImages;
        protected static readonly Dictionary<Type, object> TypeImages = new Dictionary<Type, object>();

        public static void RegisterPlugin([NotNull] Type type)
        {
            if (type.GetConstructor(Type.EmptyTypes) is null)
                throw new ArgumentException("The given type does not have a parameterless constructor.", nameof(type));

            if (!typeof(AssetsPlugin).IsAssignableFrom(type))
                throw new ArgumentException("The given type does not inherit from AssetsPlugin.", nameof(type));

            if (RegisteredPlugins.Any(p => p.GetType() == type))
                throw new InvalidOperationException("The plugin type is already registered.");

            var plugin = (AssetsPlugin) Activator.CreateInstance(type);
            RegisteredPlugins.Add(plugin);
        }

        public void RegisterAssetViewModelTypes([NotNull] IDictionary<Type, Type> assetViewModelTypes)
        {
            var pluginAssembly = GetType().Assembly;
            foreach (var type in pluginAssembly.GetTypes())
            {
                if (typeof(AssetViewModel).IsAssignableFrom(type))
                {
                    var attribute = type.GetCustomAttribute<AssetViewModelAttribute>();
                    if (attribute != null)
                    {
                        Type closureType = type;
                        attribute.AssetTypes.ForEach(x => assetViewModelTypes.Add(x, closureType));
                    }
                }
            }
        }

        public void RegisterAssetPropertyGraphViewModelTypes([NotNull] IDictionary<Type, Type> assetViewModelTypes)
        {
            var pluginAssembly = GetType().Assembly;
            foreach (var type in pluginAssembly.GetTypes())
            {
                if (typeof(AssetViewModel).IsAssignableFrom(type))
                {
                    var attribute = type.GetCustomAttribute<AssetViewModelAttribute>();
                    if (attribute != null)
                    {
                        Type closureType = type;
                        attribute.AssetTypes.ForEach(x => assetViewModelTypes.Add(x, closureType));
                    }
                }
            }
        }

        public void RegisterAssetEditorViewModelTypes([NotNull] IDictionary<Type, Type> assetEditorViewModelTypes)
        {
            var pluginAssembly = GetType().Assembly;
            foreach (var type in pluginAssembly.GetTypes())
            {
                if (typeof(IAssetEditorViewModel).IsAssignableFrom(type))
                {
                    var attribute = type.GetCustomAttribute<AssetEditorViewModelAttribute>();
                    if (attribute != null)
                    {
                        assetEditorViewModelTypes.Add(attribute.AssetType, type);
                    }
                }
            }
        }

        public abstract void InitializePlugin(ILogger logger);

        public abstract void InitializeSession([NotNull] SessionViewModel session);

        public abstract void RegisterPrimitiveTypes([NotNull] ICollection<Type> primitiveTypes);

        public abstract void RegisterEnumImages([NotNull] IDictionary<object, object> enumImages);

        public abstract void RegisterCopyProcessors([NotNull] ICollection<ICopyProcessor> copyProcessors, SessionViewModel session);

        public abstract void RegisterPasteProcessors([NotNull] ICollection<IPasteProcessor> pasteProcessors, SessionViewModel session);

        public abstract void RegisterPostPasteProcessors([NotNull] ICollection<IAssetPostPasteProcessor> postePasteProcessors, SessionViewModel session);

        public abstract void RegisterTemplateProviders([NotNull] ICollection<ITemplateProvider> templateProviders);

        protected internal virtual void SessionLoaded(SessionViewModel session)
        {
            // Intentionally does nothing
        }

        protected internal virtual void SessionDisposed(SessionViewModel session)
        {
            // Intentionally does nothing
        }
    }
}
