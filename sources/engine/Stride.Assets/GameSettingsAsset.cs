// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Stride.Core;
using Stride.Core.Mathematics;
using Stride.Core.Annotations;
using Stride.Core.Reflection;
using Stride.Core.Yaml.Serialization;
using Stride.Core.Serialization.Contents;
using Stride.Core.Assets;
using Stride.Data;
using Stride.Engine;
using Stride.Engine.Design;
using Stride.Graphics;
using Stride.Rendering.Compositing;

namespace Stride.Assets
{
    /// <summary>
    ///   Represents a collection of settings for a game, like the default scene, resolution, graphics profile, etc.
    /// </summary>
    [DataContract("GameSettingsAsset")]
    [AssetDescription(FileExtension, AlwaysMarkAsRoot = true, AllowArchetype = false)]
    [ContentSerializer(typeof(DataContentSerializer<GameSettingsAsset>))]
    [AssetContentType(typeof(GameSettings))]
    [CategoryOrder(4050, "Splash screen")]
    [NonIdentifiableCollectionItems]
    [AssetFormatVersion(StrideConfig.PackageName, CurrentVersion, "2.1.0.3")]
    [AssetUpgrader(StrideConfig.PackageName, "2.1.0.3", "3.1.0.1", typeof(RenderingSplitUpgrader))]
    public partial class GameSettingsAsset : Asset
    {
        private const string CurrentVersion = "3.1.0.1";

        /// <summary>
        ///   The default file extension used by the <see cref="GameSettingsAsset"/>.
        /// </summary>
        public const string FileExtension = ".sdgamesettings";

        public const string GameSettingsLocation = GameSettings.AssetUrl;

        public const string DefaultSceneLocation = "MainScene";

        /// <summary>
        ///   Gets or sets the default <see cref="Scene"/>.
        /// </summary>
        /// <userdoc>The default <see cref="Scene"/> loaded when the game starts.</userdoc>
        [DataMember(1000)]
        public Scene DefaultScene { get; set; }

        [DataMember(1500)]
        public GraphicsCompositor GraphicsCompositor { get; set; }

        /// <userdoc>
        ///   Gets or sets the image (for example, with a logo) displayed as the splash screen when the game starts.
        /// </userdoc>
        [Display("Texture", "Splash screen")]
        [DataMember(5000)]
        public Texture SplashScreenTexture { get; set; }

        /// <userdoc>
        ///   Gets or sets the color the splash screen fades out to.
        /// </userdoc>
        [Display("Color", "Splash screen")]
        [DataMember(5050)]
        public Color SplashScreenColor { get; set; } = Color.Black;

        [DataMember(2000)]
        [MemberCollection(ReadOnly = false, NotNullItems = true)]
        public List<Configuration> Defaults { get; } = new List<Configuration>();

        [DataMember(3000)]
        [Category]
        public List<ConfigurationOverride> Overrides { get; } = new List<ConfigurationOverride>();

        [DataMember(4000)]
        [Category]
        public List<string> PlatformFilters { get; } = new List<string>();

        /// <summary>
        ///   Tries to get the requested <see cref="Configuration"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="Configuration"/> to get.</typeparam>
        /// <returns>The configuration object as requested by <typeparamref name="T"/>, or <c>null</c> if it doesn't exist.</returns>
        public T TryGet<T>() where T : Configuration
        {
            foreach (var x in Defaults)
            {
                if (x?.GetType() == typeof(T))
                    return (T)x;
            }

            return null;
        }

        public T GetOrCreate<T>() where T : Configuration, new()
        {
            Configuration first = null;
            foreach (var x in Defaults)
            {
                if (x is T t)
                {
                    first = t;
                    break;
                }
            }
            var settings = (T) first;
            if (settings != null)
                return settings;

            settings = ObjectFactoryRegistry.NewInstance<T>();
            Defaults.Add(settings);
            return settings;
        }

        public T GetOrDefault<T>() where T : Configuration, new()
        {
            return TryGet<T>() ?? ObjectFactoryRegistry.NewInstance<T>();
        }

        public T GetOrCreate<T>(PlatformType platform) where T : Configuration, new()
        {
            ConfigPlatforms configPlatform;
            switch (platform)
            {
                case PlatformType.Windows:
                    configPlatform = ConfigPlatforms.Windows;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(platform), platform, null);
            }

            var platVersion = Overrides.FirstOrDefault(x => x != null &&
                                                            x.Platforms.HasFlag(configPlatform) &&
                                                            x.Configuration is T);
            if (platVersion != null)
            {
                return (T) platVersion.Configuration;
            }

            return GetOrCreate<T>();
        }

        // In 3.1, Stride.Engine was splitted into a sub-assembly Stride.Rendering
        private class RenderingSplitUpgrader : AssetUpgraderBase
        {
            protected override void UpgradeAsset(AssetMigrationContext context, PackageVersion currentVersion, PackageVersion targetVersion, dynamic asset, PackageLoadingAssetFile assetFile, OverrideUpgraderHint overrideHint)
            {
                YamlNode assetNode = asset.Node;
                foreach (var node in assetNode.AllNodes)
                {
                    if (node.Tag == "!Stride.Streaming.StreamingSettings,Stride.Engine")
                    {
                        node.Tag = node.Tag.Replace(",Stride.Engine", ",Stride.Rendering");
                    }
                }
            }
        }
    }
}
