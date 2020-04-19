// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.GameStudio.Services;
using Xenko.Core.Assets.Editor.View;
using Xenko.Core.Extensions;
using Xenko.Core.Presentation.Services;
using Xenko.Core.Presentation.Windows;

namespace Xenko.GameStudio.View
{
    public class XenkoDialogService : EditorDialogService, IXenkoDialogService
    {
        public XenkoDialogService(IDispatcherService dispatcher, string applicationName)
            : base(dispatcher, applicationName)
        {
        }

        public ICredentialsDialog CreateCredentialsDialog()
        {
            return new CredentialsDialog(this);
        }

        public void ShowAboutPage()
        {
            var page = new AboutPage(this);
            page.ShowModal().Forget();
        }

    }
}
