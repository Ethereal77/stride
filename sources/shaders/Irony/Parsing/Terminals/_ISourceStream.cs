// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2011 Irony - Roman Ivantsov
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stride.Irony.Parsing {
  //
  /// <summary>
  /// Interface for Terminals to access the source stream and produce tokens.
  /// </summary>
  public interface ISourceStream {


    /// <summary>
    /// Gets or sets the current preview position in the source file. Must be greater or equal to Location.Position
    /// </summary>
    int PreviewPosition { get; set; }
    /// <summary>
    /// Gets a char at preview position
    /// </summary>
    char PreviewChar { get; }


    /// <summary>
    /// Creates a new token based on current preview position and sets its Value field.
    /// </summary>
    /// <param name="terminal">A terminal associated with the token.</param>
    /// <param name="value">The value associated with the token.</param>
    /// <returns>New token.</returns>
    Token CreateToken(Terminal terminal, object value);


    /// <summary>
    /// Tries to match the symbol with the text at current preview position.
    /// </summary>
    /// <param name="symbol">A symbol to match</param>
    /// <param name="ignoreCase">True if char casing should be ignored.</param>
    /// <returns>True if there is a match; otherwise, false.</returns>
    bool MatchSymbol(string symbol, bool ignoreCase);

  }//interface


}
