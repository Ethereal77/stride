// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2011 Irony - Roman Ivantsov
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;

namespace Irony.GrammarExplorer {
  class Program {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main() {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
      AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
      Application.Run(new fmGrammarExplorer());
    }

    static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e) {
      fmShowException.ShowException(e.Exception);
      Debug.Write("Exception!: ############################################## \n" + e.Exception.ToString());
    }

    static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
      Exception ex = e.ExceptionObject as Exception;
      string message = (ex == null ? e.ExceptionObject.ToString() : ex.Message);
      if (ex == null) {
        Debug.Write("Exception!: ############################################## \n" + e.ExceptionObject.ToString());
        MessageBox.Show(message, "Exception");
      } else {
        fmShowException.ShowException(ex);
      }
    }

  }
}