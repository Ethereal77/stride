// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;

using Stride.Core.Annotations;

namespace Stride.Rendering
{
    /// <summary>
    /// A collection of <see cref="RenderObject"/>.
    /// </summary>
    public class RenderObjectCollection : ICollection<RenderObject>, IReadOnlyList<RenderObject>
    {
        private readonly VisibilityGroup visibilityGroup;
        private readonly List<RenderObject> items = new List<RenderObject>();

        internal RenderObjectCollection(VisibilityGroup visibilityGroup)
        {
            this.visibilityGroup = visibilityGroup;
        }

        public void Add(RenderObject renderObject)
        {
            visibilityGroup.AddRenderObject(items, renderObject);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(RenderObject renderObject)
        {
            return items.Contains(renderObject);
        }

        public void CopyTo(RenderObject[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public bool Remove([NotNull] RenderObject renderObject)
        {
            return visibilityGroup.RemoveRenderObject(items, renderObject);
        }

        public int Count => items.Count;

        public bool IsReadOnly => false;

        public List<RenderObject>.Enumerator GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator<RenderObject> IEnumerable<RenderObject>.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)items).GetEnumerator();
        }

        public RenderObject this[int index] => items[index];
    }
}
