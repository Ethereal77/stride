// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using NuGet.Common;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Stride.NuGetResolver
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SplashScreenWindow : Window
    {
        public SplashScreenWindow()
        {
            this.InitializeComponent();
        }

        public void AppendMessage(LogLevel level, string message)
        {
            if (level == LogLevel.Error)
            {
                CloseButton.Visibility = Visibility.Visible;
                Message.Text = "Error restoring NuGet packages!";
                Message.Foreground = new SolidColorBrush(Colors.Red);
            }
            Log.AppendText($"[{level}] {message}{Environment.NewLine}");
            Log.ScrollToEnd();
        }

        public void CloseCommand(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
