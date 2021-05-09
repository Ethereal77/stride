// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Assets;
using Stride.Core.Annotations;
using Stride.Core.Mathematics;
using Stride.Core.Serialization;
using Stride.Assets.Textures;
using Stride.Audio;
using Stride.Graphics;
using Stride.Navigation;
using Stride.Physics;
using Stride.Streaming;

namespace Stride.Assets
{
    public class GameSettingsFactory : AssetFactory<GameSettingsAsset>
    {
        [NotNull]
        public static GameSettingsAsset Create()
        {
            var asset = new GameSettingsAsset();

            asset.SplashScreenTexture = AttachedReferenceManager.CreateProxyObject<Texture>(new AssetId("d26edb11-10bd-403c-b3c2-9c7fcccf25e5"), "StrideDefaultSplashScreen");
            asset.SplashScreenColor = Color.Black;

            //add default filters, todo maybe a config file somewhere is better
            asset.PlatformFilters.Add("Intel(R) HD Graphics");

            asset.GetOrCreate<AudioEngineSettings>();
            asset.GetOrCreate<EditorSettings>();
            asset.GetOrCreate<RenderingSettings>();
            asset.GetOrCreate<StreamingSettings>();
            asset.GetOrCreate<TextureSettings>();

            return asset;
        }

        /// <inheritdoc/>
        [NotNull]
        public override GameSettingsAsset New()
        {
            return Create();
        }
    }
}
