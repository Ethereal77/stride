// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;

using Stride.Core;
using Stride.Core.Extensions;
using Stride.Core.Annotations;
using Stride.Core.Diagnostics;
using Stride.Core.Reflection;
using Stride.Core.Packages;
using Stride.Core.Assets;
using Stride.Core.Assets.Templates;
using Stride.Core.Assets.Editor.Services;
using Stride.Core.Assets.Editor.ViewModel;
using Stride.Core.Assets.Editor.Components.Properties;
using Stride.Assets.Presentation.AssetEditors.AssetHighlighters;
using Stride.Assets.Presentation.AssetEditors.EntityHierarchyEditor.EntityFactories;
using Stride.Assets.Presentation.AssetEditors.Gizmos;
using Stride.Assets.Presentation.NodePresenters.Commands;
using Stride.Assets.Presentation.NodePresenters.Updaters;
using Stride.Assets.Presentation.SceneEditor.Services;
using Stride.Assets.Presentation.ViewModel;
using Stride.Assets.Presentation.ViewModel.CopyPasteProcessors;
using Stride.Engine;
using Stride.Editor;

namespace Stride.Assets.Presentation
{
    public sealed class StrideDefaultAssetsPlugin : StrideAssetsPlugin
    {
        #region Component Type Comparer

        /// <summary>
        ///   Represents an <see cref="EqualityComparer<>"/> for Entity Component <see cref="Type"/>s.
        /// </summary>
        private sealed class ComponentTypeComparer : EqualityComparer<Type>
        {
            public new static readonly ComponentTypeComparer Default = new();

            /// <summary>
            ///   Compares two component <see cref="Type"/>s and determines whether the types match with the following criteria:
            ///   <list type="bullet">
            ///     <item>Both types are identical.</item>
            ///     <item>
            ///       The first type is a subclass of the second type (e.g. <see cref="StartupScript"/> is a subclass of <see cref="ScriptComponent"/>).
            ///     </item>
            ///   </list>
            /// </summary>
            public override bool Equals([NotNull] Type x, [NotNull] Type y)
            {
                return ReferenceEquals(x, y) || x.IsSubclassOf(y); // && y.IsSubclassOf(typeof(EntityComponent))
            }

            public override int GetHashCode(Type obj)
            {
                // Must all match the same hash so that Equals is called
                return 1;
            }
        }

        #endregion

        private EffectCompilerServerSession effectCompilerServerSession;

        private static ResourceDictionary imageDictionary;
        private static ResourceDictionary animationPropertyTemplateDictionary;
        private static ResourceDictionary entityPropertyTemplateDictionary;
        private static ResourceDictionary materialPropertyTemplateDictionary;
        private static ResourceDictionary skeletonTemplateDictionary;
        private static ResourceDictionary spriteFontTemplateDictionary;
        private static ResourceDictionary uiTemplateDictionary;
        private static ResourceDictionary graphicsCompositorTemplateDictionary;
        private static ResourceDictionary visualScriptingTemplateDictionary;
        private static ResourceDictionary visualScriptingGraphTemplatesDictionary;

        private static readonly Dictionary<Type, Type> GizmoTypes = new();
        private static readonly Dictionary<Type, Type> AssetHighlighterTypes = new();

        public static IReadOnlyDictionary<Type, Type> GizmoTypeDictionary => GizmoTypes;

        public static IReadOnlyDictionary<Type, Type> AssetHighlighterTypesDictionary => AssetHighlighterTypes;

        public static List<EntityFactoryCategory> EntityFactoryCategories { get; private set; }

        public static IReadOnlyList<(Type type, int order)> ComponentOrders { get; private set; } = new List<(Type, int)>();

        public StrideDefaultAssetsPlugin()
        {
            ProfileSettings.Add(new PackageSettingsEntry(GameUserSettings.Effect.EffectCompilation, TargetPackage.Executable));
            ProfileSettings.Add(new PackageSettingsEntry(GameUserSettings.Effect.RecordUsedEffects, TargetPackage.Executable));

            LoadDefaultTemplates();
        }

        // TODO: Currently hardcoded, this will need to change with plugin system
        private static readonly (string Name, string Version)[] DefaultPackages = new[]
            {
                ( Name: "Stride.Assets.Presentation",  Version: StrideVersion.NuGetVersion ),
                ( Name: "Stride.SpriteStudio.Offline", Version: StrideVersion.NuGetVersion ),
                ( Name: "Stride.Samples.Templates",    Version: Samples.Templates.ThisPackageVersion.Current )
            };

        public static void LoadDefaultTemplates()
        {
            // Load templates
            foreach (var packageInfo in DefaultPackages)
            {
                var logger = new LoggerResult();
                var packageFile = PackageStore.Instance.GetPackageFileName(packageInfo.Name, new PackageVersionRange(new PackageVersion(packageInfo.Version)));
                var package = Package.Load(logger, packageFile.ToWindowsPath());
                if (logger.HasErrors)
                    throw new InvalidOperationException($"Could not load package {packageInfo.Name}:{Environment.NewLine}{logger.ToText()}");

                TemplateManager.RegisterPackage(package);
            }
        }

        /// <inheritdoc />
        protected override void Initialize(ILogger logger)
        {
            imageDictionary = (ResourceDictionary) Application.LoadComponent(new Uri("/Stride.Assets.Presentation;component/View/ImageDictionary.xaml", UriKind.RelativeOrAbsolute));
            animationPropertyTemplateDictionary = (ResourceDictionary) Application.LoadComponent(new Uri("/Stride.Assets.Presentation;component/View/AnimationPropertyTemplates.xaml", UriKind.RelativeOrAbsolute));
            entityPropertyTemplateDictionary = (ResourceDictionary) Application.LoadComponent(new Uri("/Stride.Assets.Presentation;component/View/EntityPropertyTemplates.xaml", UriKind.RelativeOrAbsolute));
            materialPropertyTemplateDictionary = (ResourceDictionary) Application.LoadComponent(new Uri("/Stride.Assets.Presentation;component/View/MaterialPropertyTemplates.xaml", UriKind.RelativeOrAbsolute));
            skeletonTemplateDictionary = (ResourceDictionary) Application.LoadComponent(new Uri("/Stride.Assets.Presentation;component/View/SkeletonPropertyTemplates.xaml", UriKind.RelativeOrAbsolute));
            spriteFontTemplateDictionary = (ResourceDictionary) Application.LoadComponent(new Uri("/Stride.Assets.Presentation;component/View/SpriteFontPropertyTemplates.xaml", UriKind.RelativeOrAbsolute));
            uiTemplateDictionary = (ResourceDictionary) Application.LoadComponent(new Uri("/Stride.Assets.Presentation;component/View/UIPropertyTemplates.xaml", UriKind.RelativeOrAbsolute));
            graphicsCompositorTemplateDictionary = (ResourceDictionary) Application.LoadComponent(new Uri("/Stride.Assets.Presentation;component/View/GraphicsCompositorTemplates.xaml", UriKind.RelativeOrAbsolute));
            visualScriptingTemplateDictionary = (ResourceDictionary) Application.LoadComponent(new Uri("/Stride.Assets.Presentation;component/View/VisualScriptingTemplates.xaml", UriKind.RelativeOrAbsolute));
            visualScriptingGraphTemplatesDictionary = (ResourceDictionary) Application.LoadComponent(new Uri("/Stride.Assets.Presentation;component/AssetEditors/VisualScriptEditor/Views/GraphTemplates.xaml", UriKind.RelativeOrAbsolute));

            // Make Visual Script colors available to StaticResourceConverter
            Application.Current.Resources.MergedDictionaries.Add(imageDictionary);

            // Make script editor styles and icons available to StaticResourceConverter
            Application.Current.Resources.MergedDictionaries.Add((ResourceDictionary) Application.LoadComponent(new Uri("/Stride.Assets.Presentation;component/AssetEditors/ScriptEditor/Resources/Icons.xaml", UriKind.RelativeOrAbsolute)));
            Application.Current.Resources.MergedDictionaries.Add((ResourceDictionary) Application.LoadComponent(new Uri("/Stride.Assets.Presentation;component/AssetEditors/ScriptEditor/Resources/ThemeScriptEditor.xaml", UriKind.RelativeOrAbsolute)));

            var entityFactories = new Core.Collections.SortedList<EntityFactoryCategory, EntityFactoryCategory>();
            var assemblyEntityFactories = Assembly.GetExecutingAssembly().GetTypes()
                                          .Where(t => typeof(IEntityFactory).IsAssignableFrom(t) &&
                                                       t.GetConstructor(Type.EmptyTypes) != null);

            foreach (var factoryType in assemblyEntityFactories)
            {
                var display = factoryType.GetCustomAttribute<DisplayAttribute>();
                if (display is null)
                    continue;

                EntityFactoryCategory category;
                var existing = entityFactories.FirstOrDefault(x => x.Key.Name == display.Category);
                if (existing.Key is null)
                {
                    category = new EntityFactoryCategory(display.Category);
                    entityFactories.Add(category, category);
                }
                else
                    category = existing.Key;

                var instance = (IEntityFactory) Activator.CreateInstance(factoryType);
                // We use int.MaxValue / 2 to give enough space to all factories that do not have an Order value
                category.AddFactory(instance, display.Name, display.Order ?? int.MaxValue / 2);
            }

            // Update display name of scripts to have a decent default value if user didn't set one.
            var attributeRegistry = TypeDescriptorFactory.Default.AttributeRegistry;
            var componentTypes = typeof(EntityComponent).GetInheritedInstantiableTypes()
                                 .Where(t => attributeRegistry.GetAttribute<DisplayAttribute>(t, inherit: false) is null);
            componentTypes.ForEach(t => attributeRegistry.Register(t, new DisplayAttribute(t.Name) { Expand = ExpandRule.Once }));
            AssemblyRegistry.AssemblyRegistered += (sender, e) =>
            {
                var types = e.Assembly.GetTypes()
                            .Where(t => typeof(EntityComponent).IsAssignableFrom(t) &&
                                        attributeRegistry.GetAttribute<DisplayAttribute>(t, inherit: false) is null);
                types.ForEach(t => attributeRegistry.Register(t, new DisplayAttribute(t.Name) { Expand = ExpandRule.Once }));
            };

            EntityFactoryCategories = entityFactories.Keys.ToList();

            RegisterGizmoTypes();
            RegisterAssetHighlighterTypes();
            RegisterResourceDictionary(imageDictionary);
            RegisterResourceDictionary(animationPropertyTemplateDictionary);
            RegisterResourceDictionary(entityPropertyTemplateDictionary);
            RegisterResourceDictionary(materialPropertyTemplateDictionary);
            RegisterResourceDictionary(skeletonTemplateDictionary);
            RegisterResourceDictionary(spriteFontTemplateDictionary);
            RegisterResourceDictionary(uiTemplateDictionary);
            RegisterResourceDictionary(graphicsCompositorTemplateDictionary);
            RegisterResourceDictionary(visualScriptingTemplateDictionary);
            RegisterResourceDictionary(visualScriptingGraphTemplatesDictionary);
            RegisterComponentOrders(logger);
        }

        /// <inheritdoc />
        public override void InitializeSession(SessionViewModel session)
        {
            session.ServiceProvider.RegisterService(new StrideDialogService());

            _ = new StrideAssetsViewModel(session);

            session.AssetViewProperties.RegisterNodePresenterCommand(new FetchEntityCommand());
            session.AssetViewProperties.RegisterNodePresenterCommand(new SetEntityReferenceCommand());
            session.AssetViewProperties.RegisterNodePresenterCommand(new SetComponentReferenceCommand());
            session.AssetViewProperties.RegisterNodePresenterCommand(new SetSymbolReferenceCommand());
            session.AssetViewProperties.RegisterNodePresenterCommand(new PickupEntityCommand(session));
            session.AssetViewProperties.RegisterNodePresenterCommand(new PickupEntityComponentCommand(session));
            session.AssetViewProperties.RegisterNodePresenterCommand(new EditCurveCommand(session));
            session.AssetViewProperties.RegisterNodePresenterCommand(new SkeletonNodePreserveAllCommand());
            //TODO: Add back once properly implemented.
            //session.AssetViewProperties.RegisterNodePresenterCommand(new AddNewScriptComponentCommand());

            session.AssetViewProperties.RegisterNodePresenterUpdater(new AnimationAssetNodeUpdater());
            session.AssetViewProperties.RegisterNodePresenterUpdater(new CameraSlotNodeUpdater(session));
            session.AssetViewProperties.RegisterNodePresenterUpdater(new EntityHierarchyAssetNodeUpdater());
            session.AssetViewProperties.RegisterNodePresenterUpdater(new EntityHierarchyEditorNodeUpdater());
            session.AssetViewProperties.RegisterNodePresenterUpdater(new GameSettingsAssetNodeUpdater());
            session.AssetViewProperties.RegisterNodePresenterUpdater(new GraphicsCompositorAssetNodeUpdater());
            session.AssetViewProperties.RegisterNodePresenterUpdater(new FXAAEffectNodeUpdater());
            session.AssetViewProperties.RegisterNodePresenterUpdater(new MaterialAssetNodeUpdater());
            session.AssetViewProperties.RegisterNodePresenterUpdater(new ModelAssetNodeUpdater());
            session.AssetViewProperties.RegisterNodePresenterUpdater(new ModelNodeLinkNodeUpdater());
            session.AssetViewProperties.RegisterNodePresenterUpdater(new SkeletonAssetNodeUpdater());
            session.AssetViewProperties.RegisterNodePresenterUpdater(new SpriteFontAssetNodeUpdater());
            session.AssetViewProperties.RegisterNodePresenterUpdater(new SpriteSheetAssetNodeUpdater());
            session.AssetViewProperties.RegisterNodePresenterUpdater(new UIAssetNodeUpdater());
            session.AssetViewProperties.RegisterNodePresenterUpdater(new TextureAssetNodeUpdater());
            session.AssetViewProperties.RegisterNodePresenterUpdater(new VideoAssetNodeUpdater());
            session.AssetViewProperties.RegisterNodePresenterUpdater(new UnloadableObjectPropertyNodeUpdater());
            session.AssetViewProperties.RegisterNodePresenterUpdater(new VisualScriptNodeUpdater());
            session.AssetViewProperties.RegisterNodePresenterUpdater(new NavigationNodeUpdater(session));

            // Connects to effect compiler (to import new effect permutations discovered by running the game)
            if (Core.Assets.Editor.Settings.EditorSettings.UseEffectCompilerServer.GetValue())
            {
                effectCompilerServerSession = new EffectCompilerServerSession(session);
            }

            // Extra packages to display in "add reference" dialog
            session.SuggestedPackages.Add(new PackageName(typeof(Stride.Engine.EntityComponent).Assembly.GetName().Name, new PackageVersion(StrideVersion.NuGetVersion)));
            session.SuggestedPackages.Add(new PackageName(typeof(Stride.UI.UIElement).Assembly.GetName().Name, new PackageVersion(StrideVersion.NuGetVersion)));
            session.SuggestedPackages.Add(new PackageName(typeof(Stride.Particles.Components.ParticleSystemComponent).Assembly.GetName().Name, new PackageVersion(StrideVersion.NuGetVersion)));
            session.SuggestedPackages.Add(new PackageName(typeof(Stride.Navigation.NavigationComponent).Assembly.GetName().Name, new PackageVersion(StrideVersion.NuGetVersion)));
            session.SuggestedPackages.Add(new PackageName(typeof(Stride.Physics.StaticColliderComponent).Assembly.GetName().Name, new PackageVersion(StrideVersion.NuGetVersion)));
            session.SuggestedPackages.Add(new PackageName(typeof(Stride.Video.VideoComponent).Assembly.GetName().Name, new PackageVersion(StrideVersion.NuGetVersion)));
            session.SuggestedPackages.Add(new PackageName(typeof(Stride.Voxels.Module).Assembly.GetName().Name, new PackageVersion(StrideVersion.NuGetVersion)));
            session.SuggestedPackages.Add(new PackageName(typeof(Stride.SpriteStudio.Runtime.SpriteStudioNodeLinkComponent).Assembly.GetName().Name, new PackageVersion(StrideVersion.NuGetVersion)));
        }

        /// <inheritdoc />
        public override void RegisterPrimitiveTypes(ICollection<Type> primitiveTypes)
        {
            primitiveTypes.Add(typeof(AssetReference));
        }

        /// <inheritdoc />
        public override void RegisterCopyProcessors(ICollection<ICopyProcessor> copyProcessors, SessionViewModel session)
        {
            copyProcessors.Add(new EntityComponentCopyProcessor());
        }

        /// <inheritdoc />
        public override void RegisterPasteProcessors(ICollection<IPasteProcessor> pasteProcessors, SessionViewModel session)
        {
            pasteProcessors.Add(new EntityComponentPasteProcessor());
            pasteProcessors.Add(new EntityHierarchyPasteProcessor());
            pasteProcessors.Add(new UIHierarchyPasteProcessor());
        }

        /// <inheritdoc />
        public override void RegisterPostPasteProcessors(ICollection<IAssetPostPasteProcessor> postPasteProcessors, SessionViewModel session)
        {
            postPasteProcessors.Add(new ScenePostPasteProcessor());
        }

        /// <inheritdoc />
        protected override void SessionDisposed(SessionViewModel session)
        {
            if (effectCompilerServerSession != null)
            {
                effectCompilerServerSession.Dispose();
                effectCompilerServerSession = null;
            }
            base.SessionDisposed(session);
        }

        /// <inheritdoc />
        protected override void RegisterResourceDictionary(ResourceDictionary dictionary)
        {
            base.RegisterResourceDictionary(dictionary);

            foreach (object entry in dictionary.Keys)
            {
                if (entry is Type type)
                {
                    TypeImages[type] = dictionary[entry];
                }
            }
        }

        /// <summary>
        ///   Gets the component type which has the highest order (according to <see cref="ComponentOrderAttribute"/>)
        ///   or <see langword="null"/> if none of the given <paramref name="componentTypes"/> were registered.
        /// </summary>
        /// <remarks>If two components (or more) share the same order, the last one registered will be returned.</remarks>
        /// <param name="componentTypes">Collection of component <see cref="Type"/>s from which to pick.</param>
        /// <returns>The component <see cref="Type"/> from <paramref name="componentTypes"/> with the highest order.</returns>
        [CanBeNull]
        public static Type GetHighestOrderComponent([ItemNotNull, NotNull] IEnumerable<Type> componentTypes)
        {
            return GetComponentsByOrder(componentTypes, ascending: false).FirstOrDefault();
        }

        /// <summary>
        ///   Gets the component type which has the lowest order (according to <see cref="ComponentOrderAttribute"/>)
        ///   or <see langword="null"/> if none of the given <paramref name="componentTypes"/> were registered.
        /// </summary>
        /// <remarks>If two components (or more) share the same order, the last one registered will be returned.</remarks>
        /// <param name="componentTypes">Collection of component <see cref="Type"/>s from which to pick.</param>
        /// <returns>The component <see cref="Type"/> from <paramref name="componentTypes"/> with the lowest order.</returns>
        [CanBeNull]
        public static Type GetLowestOrderComponent([ItemNotNull, NotNull] IEnumerable<Type> componentTypes)
        {
            return GetComponentsByOrder(componentTypes, ascending: true).FirstOrDefault();
        }

        /// <summary>
        ///   Returns a collection of component types ordered according to their <see cref="DisplayAttribute.Order"/>.
        /// </summary>
        /// <remarks>If two components (or more) share the same order, the last one registered will be returned first.</remarks>
        /// <param name="componentTypes">A collection of component types to reorder.</param>
        /// <param name="ascending">
        ///   <c>true</c> if the order is from the lowest order to the highest; <c>false</c> otherwise.
        /// </param>
        /// <returns>Collection of component <see cref="Type"/>s ordered by their <see cref="DisplayAttribute.Order"/>.</returns>
        [NotNull]
        public static IEnumerable<Type> GetComponentsByOrder([ItemNotNull, NotNull] IEnumerable<Type> componentTypes, bool ascending)
        {
            // NOTE: ComponentOrders contains the component type in reverse registration order (last registered first).
            // Enumerable.OrderBy and Enumerable.OrderByDescending are stable, so order is preserved.
            var filtered = ComponentOrders.Join(componentTypes, t => t.type, c => c, (t, c) => t, ComponentTypeComparer.Default);
            return (ascending
                    ? filtered.OrderBy(t => t.order)
                    : filtered.OrderByDescending(t => t.order))
                .Select(t => t.type);
        }

        private static void RegisterGizmoTypes()
        {
            var allTypes = AssetRegistry.AssetAssemblies.SelectMany(x => x.GetTypes());
            foreach (var type in allTypes)
            {
                if (typeof(IGizmo).IsAssignableFrom(type))
                {
                    var attribute = type.GetCustomAttribute<GizmoComponentAttribute>(true);
                    if (attribute != null)
                    {
                        GizmoTypes.Add(attribute.ComponentType, type);
                    }
                }
            }
        }

        private static void RegisterAssetHighlighterTypes()
        {
            var allTypes = AssetRegistry.AssetAssemblies.SelectMany(x => x.GetTypes());
            foreach (var type in allTypes)
            {
                if (typeof(AssetHighlighter).IsAssignableFrom(type))
                {
                    foreach (var attribute in type.GetCustomAttributes<AssetHighlighterAttribute>(false).NotNull())
                    {
                        AssetHighlighterTypes.Add(attribute.AssetType, type);
                    }
                }
            }
        }

        private static void RegisterComponentOrders(ILogger logger)
        {
            // TODO: Iterate on plugin assembly or register component type in the plugin registry
            var hashSet = new HashSet<int>();
            var componentTypes = AssetRegistry.AssetAssemblies.SelectMany(x => x.GetTypes().Where(y => typeof(EntityComponent).IsAssignableFrom(y)));
            var orders = new List<(Type, int)>();
            foreach (var type in componentTypes)
            {
                // Check with inheritance to ensure they have a reachable display attribute
                var attrib = TypeDescriptorFactory.Default.AttributeRegistry.GetAttribute<ComponentOrderAttribute>(type, false);
                if (attrib != null)
                {
                    // Disable logging for an order with the same value. It's an order, not a key, so it should not log any warning messages (see this with ben)
                    if (!hashSet.Add(attrib.Order))
                    {
                        var other = orders.First(t => t.Item2 == attrib.Order);
                        logger.Warning($"Two Entity Components have explicitly the same order value ({attrib.Order}): [{other.Item1}], [{type}]");
                    }
                    // Tuples are added in the order the component are registered
                    orders.Add((type, attrib.Order));
                }
            }
            // Reverse the order so that last registered component appears first
            orders.Reverse();
            ComponentOrders = orders;
        }
    }
}
