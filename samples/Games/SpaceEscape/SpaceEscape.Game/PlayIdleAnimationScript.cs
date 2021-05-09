// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Engine;

namespace SpaceEscape
{
    /// <summary>
    /// Plays the idle animation of the entity if any
    /// </summary>
    public class PlayAnimationScript : StartupScript
    {
        public string AnimationName;

        public override void Start()
        {
            var animation = Entity.Get<AnimationComponent>();
            if (animation != null)
                animation.Play(AnimationName);
        }
    }
}
