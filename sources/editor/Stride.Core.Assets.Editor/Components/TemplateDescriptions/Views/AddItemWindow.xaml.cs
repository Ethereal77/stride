// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Windows.Input;

using Xenko.Core.Assets.Editor.Components.TemplateDescriptions.ViewModels;
using Xenko.Core.Assets.Editor.Services;
using Xenko.Core.Presentation.Commands;
using Xenko.Core.Presentation.ViewModel;

namespace Xenko.Core.Assets.Editor.Components.TemplateDescriptions.Views
{
    /// <summary>
    /// Interaction logic for AddAssetWindow.xaml
    /// </summary>
    public partial class AddItemWindow : IItemTemplateDialog
    {
        private bool validated;

        public AddItemWindow(IViewModelServiceProvider serviceProvider, TemplateDescriptionCollectionViewModel templateDescriptions)
        {
            DataContext = templateDescriptions;
            InitializeComponent();
            AddItemCommand = new AnonymousCommand<ITemplateDescriptionViewModel>(serviceProvider, ValidateSelectedTemplate);
        }

        public ICommand AddItemCommand { get; }

        public ITemplateDescriptionViewModel SelectedTemplate { get; private set; }

        private void ValidateSelectedTemplate(ITemplateDescriptionViewModel template)
        {
            // Prevent re-entrancy
            if (validated)
                return;

            validated = true;
            SelectedTemplate = template;
            Result = SelectedTemplate != null ? Presentation.Services.DialogResult.Ok : Presentation.Services.DialogResult.Cancel;
            Close();
        }
    }
}
