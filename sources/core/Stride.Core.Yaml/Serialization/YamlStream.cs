// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;
using System.IO;

using Stride.Core.Yaml.Events;

namespace Stride.Core.Yaml.Serialization
{
    /// <summary>
    /// Represents an YAML stream.
    /// </summary>
    public class YamlStream : IEnumerable<YamlDocument>
    {
        private readonly IList<YamlDocument> documents = new List<YamlDocument>();

        /// <summary>
        /// Gets the documents inside the stream.
        /// </summary>
        /// <value>The documents.</value>
        public IList<YamlDocument> Documents { get { return documents; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="YamlStream"/> class.
        /// </summary>
        public YamlStream()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YamlStream"/> class.
        /// </summary>
        public YamlStream(params YamlDocument[] documents)
            : this((IEnumerable<YamlDocument>) documents)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YamlStream"/> class.
        /// </summary>
        public YamlStream(IEnumerable<YamlDocument> documents)
        {
            foreach (var document in documents)
            {
                this.documents.Add(document);
            }
        }

        /// <summary>
        /// Adds the specified document to the <see cref="Documents"/> collection.
        /// </summary>
        /// <param name="document">The document.</param>
        public void Add(YamlDocument document)
        {
            documents.Add(document);
        }

        /// <summary>
        /// Loads the stream from the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        public void Load(TextReader input)
        {
            documents.Clear();

            var parser = new Parser(input);

            EventReader events = new EventReader(parser);
            events.Expect<StreamStart>();
            while (!events.Accept<StreamEnd>())
            {
                YamlDocument document = new YamlDocument(events);
                documents.Add(document);
            }
            events.Expect<StreamEnd>();
        }

        /// <summary>
        /// Saves the stream to the specified output.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="isLastDocumentEndImplicit">If set to <c>true</c>, last <see cref="DocumentEnd"/> will be implicit.</param>
        /// <param name="bestIndent">The desired indent.</param>
        public void Save(TextWriter output, bool isLastDocumentEndImplicit = false, int bestIndent = Emitter.MinBestIndent)
        {
            var emitter = new Emitter(output, bestIndent);

            emitter.Emit(new StreamStart());

            var lastDocument = documents.Count > 0 ? documents[documents.Count - 1] : null;
            foreach (var document in documents)
            {
                bool isDocumentEndImplicit = isLastDocumentEndImplicit && document == lastDocument;
                document.Save(emitter, isDocumentEndImplicit);
            }

            emitter.Emit(new StreamEnd());
        }

        /// <summary>
        /// Accepts the specified visitor by calling the appropriate Visit method on it.
        /// </summary>
        /// <param name="visitor">
        /// A <see cref="IYamlVisitor"/>.
        /// </param>
        public void Accept(IYamlVisitor visitor)
        {
            visitor.Visit(this);
        }

        #region IEnumerable<YamlDocument> Members

        /// <summary />
        public IEnumerator<YamlDocument> GetEnumerator()
        {
            return documents.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}