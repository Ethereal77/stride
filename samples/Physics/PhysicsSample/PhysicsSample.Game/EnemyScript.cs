// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Animations;
using Stride.Engine;
using Stride.Rendering.Sprites;

namespace PhysicsSample
{
    /// <summary>
    /// This simple script will start the sprite idle animation
    /// </summary>
    public class EnemyScript : StartupScript
    {
        public override void Start()
        {
            var spriteComponent = Entity.Get<SpriteComponent>();
            var sheet = ((SpriteFromSheet)spriteComponent.SpriteProvider).Sheet;
            SpriteAnimation.Play(spriteComponent, sheet.FindImageIndex("active0"), sheet.FindImageIndex("active1"), AnimationRepeatMode.LoopInfinite, 2);
        }
    }
}
