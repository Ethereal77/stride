// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Assets;

namespace Stride.Assets.Media
{
    public class MusicSoundFactory : AssetFactory<SoundAsset>
    {
        public override SoundAsset New()
        {
            return new SoundAsset { CompressionRatio = 10, SampleRate = 44100, Spatialized = false, StreamFromDisk = true };
        }
    }

    public class SpatializedSoundFactory : AssetFactory<SoundAsset>
    {
        public override SoundAsset New()
        {
            return new SoundAsset { CompressionRatio = 15, SampleRate = 44100, Spatialized = true, StreamFromDisk = false };
        }
    }

    public class DefaultSoundFactory : AssetFactory<SoundAsset>
    {
        public override SoundAsset New()
        {
            return new SoundAsset { CompressionRatio = 15, SampleRate = 44100, Spatialized = false, StreamFromDisk = false };
        }
    }
}
