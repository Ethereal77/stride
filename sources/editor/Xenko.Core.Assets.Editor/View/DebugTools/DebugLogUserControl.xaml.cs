// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Core.Assets.Editor.Services;
using Xenko.Core.Presentation.ViewModel;

namespace Xenko.Core.Assets.Editor.View.DebugTools
{
    /// <summary>
    /// Interaction logic for DebugLogUserControl.xaml
    /// </summary>
    public partial class DebugLogUserControl : IDebugPage
    {
        public DebugLogUserControl(LoggerViewModel loggerViewModel)
        {
            if (loggerViewModel == null) throw new ArgumentNullException("loggerViewModel");
            Logger = loggerViewModel;
            InitializeComponent();
        }

        public string Title { get; set; }

        public LoggerViewModel Logger { get; private set; }

    }
}
