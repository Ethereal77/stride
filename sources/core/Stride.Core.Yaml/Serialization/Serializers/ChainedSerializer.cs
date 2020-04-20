// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Stride.Core.Annotations;

namespace Stride.Core.Yaml.Serialization.Serializers
{
    /// <summary>
    /// An implementation of <see cref="IYamlSerializable"/> that will call the <see cref="ReadYaml"/> and <see cref="WriteYaml"/> methods
    /// of another serializer when invoked.
    /// </summary>
    public abstract class ChainedSerializer : IYamlSerializable
    {
        public ChainedSerializer Prev { get; private set; }

        public ChainedSerializer Next { get; private set; }

        public ChainedSerializer First { get { return FindBoundary(x => x.Prev); } }

        public ChainedSerializer Last { get { return FindBoundary(x => x.Next); } }

        [CanBeNull]
        public T FindPrevious<T>() where T : ChainedSerializer => FindByType<T>(x => x.Prev);

        [CanBeNull]
        public T FindNext<T>() where T : ChainedSerializer => FindByType<T>(x => x.Next);

        /// <summary>
        /// Prepends the given <see cref="ChainedSerializer"/> to this serializer.
        /// </summary>
        /// <param name="previousSerializer">The serializer to prepend.</param>
        public void Prepend([CanBeNull] ChainedSerializer previousSerializer)
        {
            // Update current Prev if non-null to target the first of the chain we're prepending
            Prev?.SetNext(previousSerializer?.First);
            previousSerializer?.First.SetPrev(Prev);
            // Set the current Prev to the given serializer
            Prev = previousSerializer;
            // Make sure that the link with the old Next of the given serializer is cleared
            previousSerializer?.Next?.SetPrev(null);
            // And set the Next of the given serializer to be this one.
            previousSerializer?.SetNext(this);
        }

        /// <summary>
        /// Appends the given <see cref="ChainedSerializer"/> to this serializer.
        /// </summary>
        /// <param name="nextSerializer">The serializer to append.</param>
        public void Append([CanBeNull] ChainedSerializer nextSerializer)
        {
            // Update current Next if non-null to target the last of the chain we're appending
            Next?.SetPrev(nextSerializer?.Last);
            nextSerializer?.Last.SetNext(Next);
            // Set the current Next to the given serializer
            Next = nextSerializer;
            // Make sure that the link with the old Prev of the given serializer is cleared
            nextSerializer?.Prev?.SetNext(null);
            // And set the Prev of the given serializer to be this one.
            nextSerializer?.SetPrev(this);
        }

        /// <inheritdoc/>
        public virtual object ReadYaml(ref ObjectContext objectContext)
        {
            if (Next == null) throw new InvalidOperationException("The last chained serializer is invoking non-existing next serializer");
            return Next.ReadYaml(ref objectContext);
        }

        /// <inheritdoc/>
        public virtual void WriteYaml(ref ObjectContext objectContext)
        {
            if (Next == null) throw new InvalidOperationException("The last chained serializer is invoking non-existing next serializer");
            Next.WriteYaml(ref objectContext);
        }

        [NotNull]
        private ChainedSerializer FindBoundary([NotNull] Func<ChainedSerializer, ChainedSerializer> navigate)
        {
            var current = this;
            while (navigate(current) != null)
            {
                current = navigate(current);
            }
            return current;
        }

        [CanBeNull]
        private T FindByType<T>([NotNull] Func<ChainedSerializer, ChainedSerializer> navigate) where T : ChainedSerializer
        {
            var current = navigate(this);
            while (current != null)
            {
                var found = current as T;
                if (found != null)
                    return found;
                current = navigate(current);
            }
            return null;
        }

        private void SetPrev(ChainedSerializer prev) => Prev = prev;
        private void SetNext(ChainedSerializer next) => Next = next;
    }
}
