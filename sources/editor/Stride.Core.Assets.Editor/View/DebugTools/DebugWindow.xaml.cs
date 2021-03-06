// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Windows;

using Stride.Core.Assets.Editor.Services;
using Stride.Core.Presentation.Collections;

namespace Stride.Core.Assets.Editor.View.DebugTools
{
    /// <summary>
    /// Interaction logic for DebugWindow.xaml
    /// </summary>
    public partial class DebugWindow
    {
        public DebugWindow()
        {
            InitializeComponent();
            Pages = new ObservableList<IDebugPage>();
            Width = Math.Min(Width, SystemParameters.WorkArea.Width);
            Height = Math.Min(Height, SystemParameters.WorkArea.Height);
            EditorDebugTools.RegisterDebugWindow(this);
        }

        public ObservableList<IDebugPage> Pages { get; private set; }
    }
}
