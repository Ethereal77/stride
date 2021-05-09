// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Windows;

using Stride.Editor.Preview.View;

namespace Stride.Assets.Presentation.Preview.Views
{
    public class TexturePreviewView : StridePreviewView
    {
        static TexturePreviewView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TexturePreviewView), new FrameworkPropertyMetadata(typeof(TexturePreviewView)));
        }
    }
}
