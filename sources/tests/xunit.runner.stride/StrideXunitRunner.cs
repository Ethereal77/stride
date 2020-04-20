// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Logging.Serilog;
using Avalonia.ReactiveUI;

using xunit.runner.stride.ViewModels;
using xunit.runner.stride.Views;

namespace xunit.runner.stride
{
    public class StrideXunitRunner
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args, Action<bool> setInteractiveMode = null) => BuildAvaloniaApp().Start((app, args2) => AppMain(app, args2, setInteractiveMode), args);

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToDebug()
                .UseReactiveUI();

        // Your application's entry point. Here you can initialize your MVVM framework, DI
        // container, etc.
        private static void AppMain(Application app, string[] args, Action<bool> setInteractiveMode)
        {
            var window = new MainWindow
            {
                DataContext = new MainWindowViewModel
                {
                    Tests =
                    {
                        SetInteractiveMode = setInteractiveMode,
                        IsInteractiveMode = true,
                    }
                }
            };

            app.Run(window);
        }
    }
}
