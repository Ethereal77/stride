// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using Stride.Core.Extensions;
using Stride.Core.Packages;
using Stride.Core.Presentation.Services;
using Stride.Core.Presentation.View;
using Stride.Core.Presentation.Windows;
using Stride.LauncherApp.Views;

namespace Stride.LauncherApp
{
    /// <summary>
    ///   Represents an instance of the Launcher, which must be a single one per user. Allows this instance to show
    ///   and hide windows, and keep the services alive.
    /// </summary>
    internal class LauncherInstance
    {
        private IDispatcherService dispatcher;
        private LauncherWindow launcherWindow;
        private NugetStore store;
        private App app;

        public LauncherErrorCode Run(bool shouldStartHidden)
        {
            dispatcher = new DispatcherService(Dispatcher.CurrentDispatcher);

            // Note: Initialize is responsible of displaying a message box in case of error
            if (!Initialize())
                return LauncherErrorCode.ErrorWhileInitializingServer;

            app = new App { ShutdownMode = ShutdownMode.OnExplicitShutdown };
            app.InitializeComponent();

            using (new WindowManager(Dispatcher.CurrentDispatcher))
            {
                dispatcher.InvokeTask(() => ApplicationEntryPoint(shouldStartHidden)).Forget();
                app.Run();
            }

            return LauncherErrorCode.Success;
        }

        internal void ShowMainWindow()
        {
            // This method can be invoked only from the dispatcher thread.
            dispatcher.EnsureAccess();

            if (launcherWindow is null)
            {
                // Create the window if we don't have it yet.
                launcherWindow = new LauncherWindow();
                launcherWindow.Initialize(store);
                launcherWindow.Closed += (s, e) => launcherWindow = null;
            }
            if (WindowManager.MainWindow is null)
            {
                // Show it if it's currently not visible
                WindowManager.ShowMainWindow(launcherWindow);
            }
            else
            {
                // Otherwise just activate it.
                if (launcherWindow.WindowState == WindowState.Minimized)
                {
                    launcherWindow.WindowState = WindowState.Normal;
                }
                launcherWindow.Activate();
            }

        }

        internal void CloseMainWindow()
        {
            // This method can be invoked only from the dispatcher thread.
            dispatcher.EnsureAccess();

            launcherWindow.Close();
        }

        internal async void ForceExit()
        {
            await Shutdown();
        }

        /// <summary>
        ///   Initializes the Launcher's service interface to handle inter-process communications.
        /// </summary>
        private bool Initialize()
        {
            // Setup the NuGet store
            store = Launcher.InitializeNugetStore();

            return true;
        }

        private async Task Shutdown()
        {
            // Close view elements first
            launcherWindow?.Close();

            // Yield so that tasks that were awaiting can complete and the server can gracefully terminate
            await Task.Yield();

            // Terminate the server and the app at last
            app.Shutdown();
        }

        private async Task ApplicationEntryPoint(bool shouldStartHidden)
        {
            var authenticated = await CheckAndPromptCredentials();

            if (!authenticated)
                Shutdown();

            if (!shouldStartHidden)
                ShowMainWindow();
        }

        /// <summary>
        ///   Asks the user for his/her credentials if no session is authenticated or has expired.
        /// </summary>
        /// <returns><c>true</c> if session was validated; <c>false</c> otherwise.</returns>
        private async Task<bool> CheckAndPromptCredentials()
        {
            // This method can be invoked only from the dispatcher thread.
            dispatcher.EnsureAccess();

            // Return whether or not we're now successfully authenticated.
            return true;
        }

        private void RequestShowMainWindow()
        {
            dispatcher.EnsureAccess(false);
            dispatcher.Invoke(ShowMainWindow);
        }

        private void RequestCloseMainWindow()
        {
            dispatcher.EnsureAccess(false);
            dispatcher.Invoke(CloseMainWindow);
        }

        private bool RequestCheckAndPromptCredentials()
        {
            dispatcher.EnsureAccess(false);
            return dispatcher.InvokeTask(CheckAndPromptCredentials).Result;
        }
    }
}
