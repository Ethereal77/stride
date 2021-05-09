// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Windows;

using Stride.Core.Assets.Editor.Services;
using Stride.Core.Assets.Editor.View;
using Stride.Core.Presentation.Services;
using Stride.Core.Presentation.ViewModel;

namespace Stride.Core.Assets.Editor.Components.FixAssetReferences.Views
{
    /// <summary>
    /// Interaction logic for FixAssetReferencesWindow.xaml
    /// </summary>
    public partial class FixAssetReferencesWindow : IFixReferencesDialog
    {
        public FixAssetReferencesWindow(IViewModelServiceProvider serviceProvider)
        {
            InitializeComponent();
            Width = Math.Min(Width, SystemParameters.WorkArea.Width);
            Height = Math.Min(Height, SystemParameters.WorkArea.Height);
        }

        /// <inheritdoc/>
        public void ApplyReferenceFixes()
        {
            var viewModel = (FixAssetReferencesViewModel)DataContext;
            viewModel.ProcessFixes();
        }
    }
}
