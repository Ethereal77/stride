// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Windows;

namespace Xenko.GameStudio
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
