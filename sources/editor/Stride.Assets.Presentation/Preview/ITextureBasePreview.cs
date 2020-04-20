// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;

using Stride.Core.Mathematics;

namespace Stride.Assets.Presentation.Preview
{
    public interface ITextureBasePreview
    {
        float SpriteScale { get; set; }

        event EventHandler SpriteScaleChanged;

        IEnumerable<int> GetAvailableMipMaps();

        void DisplayMipMap(int parseMipMapLevel);

        void ZoomIn(Vector2? centerPosition);

        void ZoomOut(Vector2? centerPosition);

        void FitOnScreen();

        void ScaleToRealSize();
    }
}
