// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2011 Irony - Roman Ivantsov
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Irony.GrammarExplorer {
  public partial class fmShowException : Form {
    public fmShowException() {
      InitializeComponent();
    }
    public static void ShowException(Exception ex) {
      fmShowException fm = new fmShowException();
      fm.txtException.Text = ex.ToString();
      fm.txtException.Select(0, 0);
      fm.Show();
    }
  }
}