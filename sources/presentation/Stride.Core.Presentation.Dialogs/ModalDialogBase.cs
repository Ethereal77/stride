// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Threading.Tasks;

using Microsoft.WindowsAPICodePack.Dialogs;

using Stride.Core.Annotations;
using Stride.Core.Presentation.Services;

namespace Stride.Core.Presentation.Dialogs
{
    public abstract class ModalDialogBase : IModalDialogInternal
    {
        private readonly IDispatcherService dispatcher;
        protected CommonFileDialog Dialog;

        protected ModalDialogBase([NotNull] IDispatcherService dispatcher)
        {
            if (dispatcher == null) throw new ArgumentNullException(nameof(dispatcher));
            this.dispatcher = dispatcher;
        }

        /// <param name="result"></param>
        /// <inheritdoc/>
        public void RequestClose(DialogResult result)
        {
            throw new NotSupportedException("RequestClose is not supported for this dialog.");
        }

        /// <inheritdoc/>
        public object DataContext { get; set; }

        /// <inheritdoc/>
        public DialogResult Result { get; set; }

        [NotNull]
        protected Task InvokeDialog()
        {
            return dispatcher.InvokeAsync(() =>
            {
                var result = Dialog.ShowDialog();
                switch (result)
                {
                    case CommonFileDialogResult.None:
                        Result = DialogResult.None;
                        break;
                    case CommonFileDialogResult.Ok:
                        Result = DialogResult.Ok;
                        break;
                    case CommonFileDialogResult.Cancel:
                        Result = DialogResult.Cancel;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            });
        }

        [NotNull]
        public abstract Task<DialogResult> ShowModal();
    }
}
