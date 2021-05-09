// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Stride.Core.Yaml.Events;

namespace Stride.Core.Yaml
{
    public class MemoryParser : IParser
    {
        private readonly IList<ParsingEvent> parsingEvents;
        private int position = -1;
        private ParsingEvent current;

        public MemoryParser(IList<ParsingEvent> parsingEvents)
        {
            this.parsingEvents = parsingEvents;
        }

        public IList<ParsingEvent> ParsingEvents => parsingEvents;

        /// <inheritdoc/>
        public ParsingEvent Current => current;

        /// <inheritdoc/>
        public bool IsEndOfStream => position >= parsingEvents.Count;

        public int Position
        {
            get { return position; }
            set
            {
                position = value;
                current = (position >= 0) ? parsingEvents[position] : null;
            }
        }

        public bool MoveNext()
        {
            if (++position < parsingEvents.Count)
            {
                current = parsingEvents[position];
                return true;
            }

            return false;
        }
    }
}
