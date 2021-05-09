// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.Presentation.Tests.WPF
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow
    {
        public TestWindow(string title)
            : this()
        {
            Title = title;
        }

        public TestWindow()
        {
            InitializeComponent();
        }

        public event EventHandler<EventArgs> Shown;

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            Shown?.Invoke(this, EventArgs.Empty);
        }
    }
}
