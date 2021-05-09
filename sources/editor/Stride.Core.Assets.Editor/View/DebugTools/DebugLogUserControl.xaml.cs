// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Assets.Editor.Services;
using Stride.Core.Presentation.ViewModel;

namespace Stride.Core.Assets.Editor.View.DebugTools
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
