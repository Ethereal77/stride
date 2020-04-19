// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;
using System.Collections.ObjectModel;

using Xenko.Core.Yaml.Tokens;

namespace Xenko.Core.Yaml
{
    /// <summary>
    /// Collection of <see cref="TagDirective"/>.
    /// </summary>
    public class TagDirectiveCollection : KeyedCollection<string, TagDirective>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TagDirectiveCollection"/> class.
        /// </summary>
        public TagDirectiveCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TagDirectiveCollection"/> class.
        /// </summary>
        /// <param name="tagDirectives">Initial content of the collection.</param>
        public TagDirectiveCollection(IEnumerable<TagDirective> tagDirectives)
        {
            foreach (var tagDirective in tagDirectives)
            {
                Add(tagDirective);
            }
        }

        /// <summary/>
        protected override string GetKeyForItem(TagDirective item)
        {
            return item.Handle;
        }

        /// <summary>
        /// Gets a value indicating whether the collection contains a directive with the same handle
        /// </summary>
        public new bool Contains(TagDirective directive)
        {
            return Contains(GetKeyForItem(directive));
        }
    }
}