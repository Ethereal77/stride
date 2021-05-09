// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Irony.Parsing;

namespace Stride.Core.Shaders.Grammar
{
    /// <summary>
    /// A Custom Scanner used for Irony
    /// </summary>
    internal class CustomScanner : Scanner
    {
        private Tokenizer tokenizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomScanner"/> class.
        /// </summary>
        /// <param name="tokenizer">The tokenizer.</param>
        public CustomScanner(Tokenizer tokenizer)
        {
            this.tokenizer = tokenizer;
        }

        /// <inheritdoc/>
        public override SourceLocation Location
        {
            get
            {
                return tokenizer.Location;
            }

            set
            {
                tokenizer.Location = value;
            }
        }

        /// <inheritdoc/>
        public override void SetSourceText(string sourceText, string sourceFileName)
        {
            tokenizer.SetSourceText(sourceText, sourceFileName);
        }

        /// <inheritdoc/>
        protected override void NextToken()
        {
            Context.CurrentToken = tokenizer.GetNextToken();
        }

        /// <inheritdoc/>
        protected override void PrepareInput()
        {
        }
    }
}
