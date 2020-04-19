// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Windows;

using Xenko.Core.Assets.Editor.Services;
using Xenko.Core.Assets.Editor.View;
using Xenko.Core.Presentation.Services;
using Xenko.Core.Presentation.ViewModel;

namespace Xenko.Core.Assets.Editor.Components.FixAssetReferences.Views
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
