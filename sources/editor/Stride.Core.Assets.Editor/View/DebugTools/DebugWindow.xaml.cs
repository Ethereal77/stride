// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Windows;

using Xenko.Core.Assets.Editor.Services;
using Xenko.Core.Presentation.Collections;

namespace Xenko.Core.Assets.Editor.View.DebugTools
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
