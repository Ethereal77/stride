// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Engine;
using Xenko.Graphics;
using Xenko.UI;
using Xenko.UI.Controls;
using Xenko.UI.Panels;

namespace AnimatedModel
{
    public class UIScript : StartupScript
    {
        public Entity Knight;

        public SpriteFont Font;

        public override void Start()
        {
            base.Start();

            // Bind the buttons
            var page = Entity.Get<UIComponent>().Page;

            var btnIdle = page.RootElement.FindVisualChildOfType<Button>("ButtonIdle");
            btnIdle.Click += (s, e) => Knight.Get<AnimationComponent>().Crossfade("Idle", TimeSpan.FromSeconds(0.25));

            var btnRun = page.RootElement.FindVisualChildOfType<Button>("ButtonRun");
            btnRun.Click += (s, e) => Knight.Get<AnimationComponent>().Crossfade("Run", TimeSpan.FromSeconds(0.25));

            // Set the default animation
            Knight.Get<AnimationComponent>().Play("Run");
        }        
    }
}
