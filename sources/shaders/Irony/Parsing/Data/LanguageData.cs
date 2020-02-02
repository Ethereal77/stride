// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2011 Irony - Roman Ivantsov
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Irony.Parsing.Construction;

namespace Irony.Parsing
{
    /// <summary>
    /// Describes a language.
    /// </summary>
    public class LanguageData
    {
        #region Constants and Fields

        /// <summary>
        /// Grammar errors.
        /// </summary>
        public readonly GrammarErrorList Errors = new GrammarErrorList();

        /// <summary>
        /// The linked Grammar
        /// </summary>
        public Grammar Grammar { get; private set; }

        /// <summary>
        /// Raw data extracted from the grammar.
        /// </summary>
        public GrammarData GrammarData { get; private set; }

        /// <summary>
        /// Data for the parser.
        /// </summary>
        public ParserData ParserData { get; private set; }

        /// <summary>
        /// Time in ms to build a scanner.
        /// </summary>
        public long ConstructionTime;

        /// <summary>
        /// Error level.
        /// </summary>
        public GrammarErrorLevel ErrorLevel = GrammarErrorLevel.NoError;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes the specified grammar.
        /// </summary>
        /// <param name="grammar">The grammar.</param>
        public LanguageData(Grammar grammar)
        {
            Grammar = grammar;
            GrammarData = new GrammarData(this);
            ParserData = new ParserData(this);
            ConstructAll();
        }

        public virtual Scanner CreateScanner()
        {
            return null;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines whether this instance can parse.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can parse; otherwise, <c>false</c>.
        /// </returns>
        public bool CanParse()
        {
            return ErrorLevel < GrammarErrorLevel.Error;
        }

        /// <summary>
        /// Constructs all.
        /// </summary>
        public void ConstructAll()
        {
            var builder = new LanguageDataBuilder(this);
            builder.Build();
        }

        #endregion
    }
}
