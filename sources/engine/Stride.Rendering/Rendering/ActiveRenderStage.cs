// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Rendering
{
    public struct ActiveRenderStage
    {
        public bool Active => EffectSelector != null;

        public EffectSelector EffectSelector;

        public ActiveRenderStage(string effectName)
        {
            EffectSelector = new EffectSelector(effectName);
        }
    }
}
