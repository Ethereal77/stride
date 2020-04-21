// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Core.Yaml.Events
{
    /// <summary>
    /// Represents a sequence end event.
    /// </summary>
    public class SequenceEnd : ParsingEvent
    {
        /// <summary>
        /// Gets a value indicating the variation of depth caused by this event.
        /// The value can be either -1, 0 or 1. For start events, it will be 1,
        /// for end events, it will be -1, and for the remaining events, it will be 0.
        /// </summary>
        public override int NestingIncrease { get { return -1; } }

        /// <summary>
        /// Gets the event type, which allows for simpler type comparisons.
        /// </summary>
        internal override EventType Type { get { return EventType.YAML_SEQUENCE_END_EVENT; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="SequenceEnd"/> class.
        /// </summary>
        /// <param name="start">The start position of the event.</param>
        /// <param name="end">The end position of the event.</param>
        public SequenceEnd(Mark start, Mark end)
            : base(start, end)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SequenceEnd"/> class.
        /// </summary>
        public SequenceEnd()
            : this(Mark.Empty, Mark.Empty)
        {
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return "Sequence end";
        }
    }
}