// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Core.Yaml.Events
{
    /// <summary>
    /// Base class for parsing events.
    /// </summary>
    public abstract class ParsingEvent
    {
        /// <summary>
        /// Gets a value indicating the variation of depth caused by this event.
        /// The value can be either -1, 0 or 1. For start events, it will be 1,
        /// for end events, it will be -1, and for the remaining events, it will be 0.
        /// </summary>
        public abstract int NestingIncrease { get; }

        /// <summary>
        /// Gets the event type, which allows for simpler type comparisons.
        /// </summary>
        internal abstract EventType Type { get; }

        private readonly Mark start;

        /// <summary>
        /// Gets the position in the input stream where the event starts.
        /// </summary>
        public Mark Start { get { return start; } }

        private readonly Mark end;

        /// <summary>
        /// Gets the position in the input stream where the event ends.
        /// </summary>
        public Mark End { get { return end; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsingEvent"/> class.
        /// </summary>
        /// <param name="start">The start position of the event.</param>
        /// <param name="end">The end position of the event.</param>
        internal ParsingEvent(Mark start, Mark end)
        {
            this.start = start;
            this.end = end;
        }
    }
}