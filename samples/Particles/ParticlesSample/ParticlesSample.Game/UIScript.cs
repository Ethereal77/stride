// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Engine;
using Xenko.Graphics;
using Xenko.UI;
using Xenko.UI.Controls;
using Xenko.UI.Panels;

namespace ParticlesSample
{
    public class UIScript : StartupScript
    {
        public Entity Knight;

        public SpriteFont Font;

        public override void Start()
        {
            base.Start();

            // Setup the UI
            Entity.Get<UIComponent>().Page.RootElement = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 10, 20, 0),
                Children = { CreateButton("Idle"), CreateButton("Run") }
            };

            // Set the default animation
            Knight.Get<AnimationComponent>().Play("Run");
        }

        /// <summary>
        /// Create a button and link the click action to the corresponding animation.
        /// </summary>
        private Button CreateButton(string animationName)
        {
            var idleButton = new Button
            {
                Content = new TextBlock
                {
                    Text = "Play " + animationName,
                    Font = Font,
                },
                Padding = new Thickness(10, 10, 10, 10),
                Margin = new Thickness(0, 0, 0, 10),
            };
            idleButton.Click += (s, e) => Knight.Get<AnimationComponent>().Crossfade(animationName, TimeSpan.FromSeconds(0.1));

            return idleButton;
        }
    }
}
