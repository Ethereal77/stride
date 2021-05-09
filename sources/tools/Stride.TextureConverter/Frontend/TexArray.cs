// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextureTools
{
    class TexArray : TexImage
    {
        public List<TexImage> Array { get; internal set; }

        internal TexArray() : base()
        {
            Array = new List<TexImage>();
        }

        public TexArray(List<TexImage> array)
        {
            Array = array;
        }

        public override Object Clone(bool CopyMemory)
        {
            TexAtlas atlas = (TexAtlas)base.Clone(CopyMemory);

            atlas.Layout = new TexLayout();
            foreach (KeyValuePair<string, TexLayout.Position> entry in Layout.TexList)
            {
                atlas.Layout.TexList.Add(entry.Key, entry.Value);
            }

            return atlas;
        }
    }
}

