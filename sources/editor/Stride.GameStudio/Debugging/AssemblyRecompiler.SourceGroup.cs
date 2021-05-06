// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.CodeAnalysis;

using Mono.Cecil;

using QuickGraph;

namespace Stride.GameStudio.Debugging
{
    partial class AssemblyRecompiler
    {
        /// <summary>
        ///   Represents a group of source <see cref="SyntaxTree"/>, and later a generated <see cref="Project"/> and compiled assemblies.
        /// </summary>
        public class SourceGroup : AdjacencyGraph<SyntaxTree, SEdge<SyntaxTree>>
        {
            /// <summary>
            ///   Gets or sets the generated Roslyn <see cref="Microsoft.CodeAnalysis.Project"/>.
            /// </summary>
            public Project Project { get; set; }

            /// <summary>
            ///   Gets or sets the assembly PE image data.
            /// </summary>
            public byte[] PE { get; set; }

            /// <summary>
            ///   Gets or sets the assembly PDB debugging data.
            /// </summary>
            public byte[] PDB { get; set; }

            /// <value>
            ///   Gets or sets a temporary assembly definition when generating it.
            /// </value>
            internal AssemblyDefinition Assembly { get; set; }


            public override string ToString() => string.Join(' ', Vertices.Select(syntaxTree => Path.GetFileName(syntaxTree.FilePath)));
        }


        /// <summary>
        ///   An object that can compare two <see cref="SourceGroup"/>s for equality.
        /// </summary>
        public class SourceGroupComparer : EqualityComparer<SourceGroup>
        {
            private static readonly SourceGroupComparer _default = new();

            /// <summary>
            ///   Gets the default comparer for <see cref="SourceGroup"/>s.
            /// </summary>
            public static new SourceGroupComparer Default => _default;


            public override bool Equals(SourceGroup x, SourceGroup y)
            {
                // Compare if two collection of SyntaxTree are the same
                // Not the best perf-wise, but should be good enough for now
                return new HashSet<SyntaxTree>(x.Vertices).SetEquals(y.Vertices);
            }

            public override int GetHashCode(SourceGroup sourceGroup)
            {
                HashCode hashCode = new();
                foreach (var vertex in sourceGroup.Vertices.OrderBy(x => x.FilePath))
                    hashCode.Add(vertex);
                return hashCode.ToHashCode();
            }
        }
    }
}
