// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Assets.Editor.Services;
using Stride.Core;
using Stride.Core.Presentation.Services;
using Stride.Core.Presentation.ViewModel;

namespace Stride.Core.Assets.Editor.Components.DebugTools.UndoRedo.Views
{
    /// <summary>
    /// Interaction logic for DebugActionStackUserControl.xaml
    /// </summary>
    public partial class DebugUndoRedoUserControl : IDebugPage, IDestroyable
    {        
        public DebugUndoRedoUserControl(IViewModelServiceProvider serviceProvider, IUndoRedoService undoRedo)
        {
            InitializeComponent();
            DataContext = new DebugUndoRedoViewModel(serviceProvider, undoRedo);
        }

        public string Title { get; set; }

        public void Destroy()
        {
            ((DebugUndoRedoViewModel)DataContext).Destroy();
        }
    }
}
