// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Windows;

namespace Stride.GameStudio
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private DataBindingExceptionRethrower exceptionRethrower;

        protected override void OnStartup(StartupEventArgs e)
        {
            exceptionRethrower = new DataBindingExceptionRethrower();
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            exceptionRethrower?.Dispose();
        }
    }
}
