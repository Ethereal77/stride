// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2011 Irony - Roman Ivantsov
// See the LICENSE.md file in the project root for full license information.

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