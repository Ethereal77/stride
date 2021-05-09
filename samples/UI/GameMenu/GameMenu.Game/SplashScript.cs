// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Linq;

using Stride.Core.Serialization;
using Stride.Engine;
using Stride.Input;

namespace GameMenu
{
    public class SplashScript : UISceneBase
    {
        public UrlReference<Scene> NextSceneUrl { get; set; }

        protected override void LoadScene()
        {
            // Allow user to resize the window with the mouse.
            Game.Window.AllowUserResizing = true;
        }

        protected override void UpdateScene()
        {
            if (Input.PointerEvents.Any(e => e.EventType == PointerEventType.Pressed))
            {
                // Next scene
                SceneSystem.SceneInstance.RootScene = Content.Load(NextSceneUrl);
                Cancel();
            }
        }
    }
}
