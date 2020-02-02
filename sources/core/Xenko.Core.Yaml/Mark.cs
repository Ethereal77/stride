// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Core.Yaml
{
    /// <summary>
    /// Represents a location inside a file
    /// </summary>
    public struct Mark
    {
        private int index;
        private int line;
        private int column;

        /// <summary>
        /// Gets / sets the absolute offset in the file
        /// </summary>
        public int Index
        {
            get { return index; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", "Index must be greater than or equal to zero.");
                }
                index = value;
            }
        }

        /// <summary>
        /// Gets / sets the number of the line
        /// </summary>
        public int Line
        {
            get { return line; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", "Line must be greater than or equal to zero.");
                }
                line = value;
            }
        }

        /// <summary>
        /// Gets / sets the index of the column
        /// </summary>
        public int Column
        {
            get { return column; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", "Column must be greater than or equal to zero.");
                }
                column = value;
            }
        }

        /// <summary>
        /// Gets a <see cref="Mark"/> with empty values.
        /// </summary>
        public static readonly Mark Empty;

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Lin: {0}, Col: {1}, Chr: {2}", line, column, index);
        }
    }
}