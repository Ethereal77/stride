// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Core.Annotations;
using Xenko.Core.Presentation.Commands;
using Xenko.Core.Presentation.ViewModel;

namespace Xenko.Core.Assets.Editor.ViewModel
{
    /// <summary>
    /// Wrapper for <see cref="ICommandBase"/> with additional information, best fitted for menu command (gesture, tooltip...).
    /// </summary>
    public sealed class MenuCommandInfo : DispatcherViewModel
    {
        private string gesture;
        private string displayName;
        private object icon;
        private string tooltip;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuCommandInfo"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider to use with this view model.</param>
        /// <param name="command">The command to wrap.</param>
        public MenuCommandInfo([NotNull] IViewModelServiceProvider serviceProvider, [NotNull] ICommandBase command)
            : base(serviceProvider)
        {
            Command = command ?? throw new ArgumentNullException(nameof(command));
        }

        /// <summary>
        /// Gets the command.
        /// </summary>
        public ICommandBase Command { get; }

        /// <summary>
        /// Gets or sets the name that will be displayed in the UI.
        /// </summary>
        public string DisplayName
        {
            get => displayName;
            set => SetValue(ref displayName, value);
        }

        /// <summary>
        /// Gets or sets the gesture text associated with this command.
        /// </summary>
        public string Gesture
        {
            get => gesture;
            set => SetValue(ref gesture, value);
        }

        /// <summary>
        /// Gets or sets the icon that appears in a <see cref="System.Windows.Controls.MenuItem"/>.
        /// </summary>
        public object Icon
        {
            get => icon;
            set => SetValue(ref icon, value);
        }

        /// <summary>
        /// Gets or sets the tooltip text that is shown when the governing UI control is hovered.
        /// </summary>
        public string Tooltip
        {
            get => tooltip;
            set => SetValue(ref tooltip, value);
        }
    }
}
