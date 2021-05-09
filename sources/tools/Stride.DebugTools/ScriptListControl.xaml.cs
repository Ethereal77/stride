// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Stride.Extensions;
using Stride.Framework.MicroThreading;
using Stride.DebugTools.ViewModels;

namespace Stride.DebugTools
{
    /// <summary>
    /// Interaction logic for ScriptListControl.xaml
    /// </summary>
    public partial class ScriptListControl : UserControl
    {
        private EngineContext engineContext;

        public ScriptListControl()
        {
            InitializeComponent();
        }

        internal void Initialize(EngineContext engineContext)
        {
            this.engineContext = engineContext;

            this.DataContext = new RootViewModel(engineContext, processInfoRenderer);
            processInfoRendererScroller.ScrollToRightEnd();
        }
    }
}
