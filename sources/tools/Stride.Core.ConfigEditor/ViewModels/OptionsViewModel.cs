// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.IO;
using System.Windows.Input;

using Stride.Core.Presentation.Commands;
using Stride.Core.Presentation.ViewModel;

namespace Stride.ConfigEditor.ViewModels
{
    public class OptionsViewModel : ViewModelBase
    {
        public Options Options { get; private set; }

        public OptionsViewModel()
        {
            Options = Options.Load() ?? new Options();

            StridePath = Options.StridePath;
            StrideConfigFilename = Options.StrideConfigFilename;

            CheckStridePath();
            CheckStrideConfigFilename();

            BrowsePathCommand = new AnonymousCommand(BrowsePath);
            BrowseConfigFileCommand = new AnonymousCommand(BrowseConfigFile);
        }

        public void SetOptionsWindow(Window window)
        {
            CloseCommand = new AnonymousCommand(window.Close);
        }

        public ICommand CloseCommand { get; private set; }
        public ICommand BrowsePathCommand { get; private set; }
        public ICommand BrowseConfigFileCommand { get; private set; }

        private void BrowsePath()
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog
            {
                Description = "Select Stride base directory",
                ShowNewFolderButton = true,
            };

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                StridePath = dialog.SelectedPath;
        }

        private void BrowseConfigFile()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Select the Stride configuration file",
                Filter = "Xml Files (*.xml)|*.xml|All Files (*.*)|*.*",
                Multiselect = false,
                CheckFileExists = true,
            };

            if (dialog.ShowDialog() == true)
                StrideConfigFilename = dialog.FileName;
        }

        private string stridePath;
        public string StridePath
        {
            get => stridePath;
            set
            {
                if (SetValue(ref stridePath, value, nameof(StridePath)))
                    CheckStridePath();
            }
        }

        private bool isStridePathValid;
        public bool IsStridePathValid
        {
            get => isStridePathValid;
            set => SetValue(ref isStridePathValid, value, nameof(IsStridePathValid));
        }

        private void CheckStridePath()
        {
            IsStridePathValid = Directory.Exists(StridePath);
        }

        private string strideConfigFilename;
        public string StrideConfigFilename
        {
            get => strideConfigFilename;
            set
            {
                if (SetValue(ref strideConfigFilename, value, nameof(StrideConfigFilename)))
                    CheckStrideConfigFilename();
            }
        }

        private bool isStrideConfigFilenameValid;
        public bool IsStrideConfigFilenameValid
        {
            get => isStrideConfigFilenameValid;
            set => SetValue(ref isStrideConfigFilenameValid, value, nameof(IsStrideConfigFilenameValid));
        }

        private void CheckStrideConfigFilename()
        {
            if (string.IsNullOrWhiteSpace(StrideConfigFilename))
            {
                IsStrideConfigFilenameValid = true;
                return;
            }

            var tempFilename = StrideConfigFilename;

            if (Path.IsPathRooted(tempFilename) == false)
                tempFilename = Path.Combine(StridePath, StrideConfigFilename);

            IsStrideConfigFilenameValid = File.Exists(tempFilename);
        }

        private ICommand acceptCommand;
        public ICommand AcceptCommand
        {
            get
            {
                if (acceptCommand is null)
                    acceptCommand = new AnonymousCommand(Accept);
                return acceptCommand;
            }
        }

        private void Accept()
        {
            if (string.IsNullOrWhiteSpace(StridePath))
            {
                MessageBox.Show("Invalid Stride Path, this field must not be empty.", "Stride Path Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Directory.Exists(StridePath) == false)
            {
                string message = string.Format("Invalid Stride Path, the directory '{0}' does not exit.", StridePath);
                MessageBox.Show(message, "Stride Path Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Options.StridePath = StridePath;
            Options.StrideConfigFilename = StrideConfigFilename;

            Options.Save();

            var handler = OptionsChanged;
            if (handler != null)
                handler();

            CloseCommand.Execute(null); // this just closes the Options window
        }

        public event Action OptionsChanged;
    }
}
