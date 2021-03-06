// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2011 Irony - Roman Ivantsov
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

namespace Stride.Irony.Parsing
{

    /// <summary>
    /// Token flags.
    /// </summary>
    public enum TokenFlags
    {
        IsIncomplete = 0x01,
    }

    /// <summary>
    /// Token category.
    /// </summary>
    public enum TokenCategory
    {
        /// <summary>
        /// Content category.
        /// </summary>
        Content,

        /// <summary>
        /// newLine, indent, dedent
        /// </summary>
        Outline,

        /// <summary>
        /// Comment category.
        /// </summary>
        Comment,

        /// <summary>
        /// Directive category.
        /// </summary>
        Directive,

        /// <summary>
        /// Error category.
        /// </summary>
        Error,
    }

    /// <summary>
    /// A List of tokens.
    /// </summary>
    public class TokenList : List<Token>
    {
    }

    /// <summary>
    /// A Stack of tokens.
    /// </summary>
    public class TokenStack : Stack<Token>
    {
    }

    /// <summary>
    /// Tokens are produced by scanner and fed to parser, optionally passing through Token filters in between.
    /// </summary>
    public class Token
    {
        private string text;

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="term">The term.</param>
        /// <param name="location">The location.</param>
        /// <param name="text">The text.</param>
        /// <param name="value">The value.</param>
        public Token(Terminal term, SourceLocation location, string text, object value)
        {
            SetTerminal(term);
            KeyTerm = term as KeyTerm;
            Location = location;
            Length = text.Length;
            this.text = text;
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="term">The term.</param>
        /// <param name="location">The location.</param>
        /// <param name="length">The length.</param>
        /// <param name="source">The source.</param>
        /// <param name="value">The value.</param>
        public Token(Terminal term, SourceLocation location, int length, string source, object value)
        {
            SetTerminal(term);
            KeyTerm = term as KeyTerm;
            Location = location;
            Length = length;
            SourceCode = source;
            Value = value;
        }

        /// <summary>
        /// Location in the source code.
        /// </summary>
        public readonly SourceLocation Location;

        /// <summary>
        /// Gets the terminal.
        /// </summary>
        public Terminal Terminal { get; private set; }

        /// <summary>
        /// Gets the Key terminal if any.
        /// </summary>
        public KeyTerm KeyTerm { get; private set; }

        /// <summary>
        /// Gets the length.
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// Gets the source code.
        /// </summary>
        /// <value>
        /// The source code.
        /// </value>
        public string SourceCode { get; private set; }

        /// <summary>
        /// Gets the text associated with this token.
        /// </summary>
        public string Text
        {
            get
            {
                if (text == null)
                {
                    text = SourceCode.Substring(Location.Position, Length);
                }
                return text;
            }
        }

        /// <summary>
        /// Get the Value associated with this token.
        /// </summary>
        public object Value;

        /// <summary>
        /// Gets the value as a string.
        /// </summary>
        public string ValueString
        {
            get
            {
                return Value == null ? string.Empty : Value.ToString();
            }
        }

        /// <summary>
        /// Get the flags
        /// </summary>
        public TokenFlags Flags;

        /// <summary>
        /// Gets the Editor info.
        /// </summary>
        public TokenEditorInfo EditorInfo;

        /// <summary>
        /// Gets the category.
        /// </summary>
        public TokenCategory Category
        {
            get { return Terminal.Category; }
        }

        /// <summary>
        /// Gets the matching opening/closing brace
        /// </summary>
        public Token OtherBrace
        {
            get;
            private set;
        }

        /// <summary>
        /// Scanner state after producing token
        /// </summary>
        public short ScannerState;

        /// <summary>
        /// Sets the terminal.
        /// </summary>
        /// <param name="terminal">The terminal.</param>
        public void SetTerminal(Terminal terminal)
        {
            Terminal = terminal;

            // Set to term's EditorInfo by default
            EditorInfo = Terminal.EditorInfo;
        }


        /// <summary>
        /// Determines whether this instance is error.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is error; otherwise, <c>false</c>.
        /// </returns>
        public bool IsError()
        {
            return Category == TokenCategory.Error;
        }

        /// <summary>
        /// Links the matching braces.
        /// </summary>
        /// <param name="openingBrace">The opening brace.</param>
        /// <param name="closingBrace">The closing brace.</param>
        public static void LinkMatchingBraces(Token openingBrace, Token closingBrace)
        {
            openingBrace.OtherBrace = closingBrace;
            closingBrace.OtherBrace = openingBrace;
        }

        /// <inheritdoc/>
        [System.Diagnostics.DebuggerStepThrough]
        public override string ToString()
        {
            return Terminal.TokenToString(this);
        }
    }
}
