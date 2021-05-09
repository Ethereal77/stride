// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Microsoft.Xaml.Behaviors;

using Stride.Core.Presentation.Controls;

namespace Stride.DebugTools.Behaviors
{
    public class TimeBarCurrentTimeSetterBehavior : Behavior<ScaleBar>
    {
        public ProcessInfoRenderer Renderer { get; set; }

        protected override void OnAttached()
        {
            if (Renderer is null)
                // throw new InvalidOperationException("The Renderer property must be set a valid value.");
                return; // Can be null at design time

            Renderer.LastFrameRender += OnRendererLastFrameRender;
        }

        protected override void OnDetaching()
        {
            Renderer.LastFrameRender -= OnRendererLastFrameRender;
        }

        private void OnRendererLastFrameRender(object sender, FrameRenderRoutedEventArgs e)
        {
            AssociatedObject.SetUnitAt(e.FrameData.EndTime, Renderer.ActualWidth);
        }
    }
}
