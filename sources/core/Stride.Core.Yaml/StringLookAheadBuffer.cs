// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Stride.Core.Yaml
{
    internal class StringLookAheadBuffer : ILookAheadBuffer
    {
        private readonly string value;
        private int currentIndex;

        public int Length { get { return value.Length; } }

        public int Position { get { return currentIndex; } }

        private bool IsOutside(int index)
        {
            return index >= value.Length;
        }

        public bool EndOfInput { get { return IsOutside(currentIndex); } }

        public StringLookAheadBuffer(string value)
        {
            this.value = value;
        }

        public char Peek(int offset)
        {
            int index = currentIndex + offset;
            return IsOutside(index) ? '\0' : value[index];
        }

        public void Skip(int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length", "The length must be positive.");
            }
            currentIndex += length;
        }
    }
}