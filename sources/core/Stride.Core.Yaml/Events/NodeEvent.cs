// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Text.RegularExpressions;

namespace Stride.Core.Yaml.Events
{
    /// <summary>
    /// Contains the behavior that is common between node events.
    /// </summary>
    public abstract class NodeEvent : ParsingEvent
    {
        internal static readonly Regex anchorValidator = new Regex(@"^[0-9a-zA-Z_\-]+$");

        private readonly string anchor;

        /// <summary>
        /// Gets the anchor.
        /// </summary>
        /// <value></value>
        public string Anchor { get { return anchor; } }

        private readonly string tag;

        /// <summary>
        /// Gets the tag.
        /// </summary>
        /// <value></value>
        public string Tag { get { return tag; } }

        /// <summary>
        /// Gets a value indicating whether this instance is canonical.
        /// </summary>
        /// <value></value>
        public abstract bool IsCanonical { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeEvent"/> class.
        /// </summary>
        /// <param name="anchor">The anchor.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="start">The start position of the event.</param>
        /// <param name="end">The end position of the event.</param>
        protected NodeEvent(string anchor, string tag, Mark start, Mark end)
            : base(start, end)
        {
            if (anchor != null)
            {
                if (anchor.Length == 0)
                {
                    throw new ArgumentException("Anchor value must not be empty.", "anchor");
                }

                if (!anchorValidator.IsMatch(anchor))
                {
                    throw new ArgumentException("Anchor value must contain alphanumerical characters only.", "anchor");
                }
            }

            if (tag != null && tag.Length == 0)
            {
                throw new ArgumentException("Tag value must not be empty.", "tag");
            }

            this.anchor = anchor;
            this.tag = tag;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeEvent"/> class.
        /// </summary>
        protected NodeEvent(string anchor, string tag)
            : this(anchor, tag, Mark.Empty, Mark.Empty)
        {
        }
    }
}