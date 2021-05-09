// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Yaml.Serialization
{
    /// <summary>
    /// The exception that is thrown when an alias references an anchor that does not exist.
    /// </summary>
    public class AnchorNotFoundException : YamlException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnchorNotFoundException" /> class.
        /// </summary>
        /// <param name="anchorAlias">The anchor alias.</param>
        public AnchorNotFoundException(string anchorAlias)
        {
            Alias = anchorAlias;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnchorNotFoundException" /> class.
        /// </summary>
        /// <param name="anchorAlias">The anchor alias.</param>
        /// <param name="message">The message.</param>
        public AnchorNotFoundException(string anchorAlias, string message)
            : base(message)
        {
            Alias = anchorAlias;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnchorNotFoundException" /> class.
        /// </summary>
        /// <param name="anchorAlias">The anchor alias.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="message">The message.</param>
        public AnchorNotFoundException(string anchorAlias, Mark start, Mark end, string message)
            : base(start, end, message)
        {
            Alias = anchorAlias;
        }

        /// <summary>
        /// Gets or sets the anchor alias.
        /// </summary>
        /// <value>The anchor alias.</value>
        public string Alias { get; private set; }
    }
}