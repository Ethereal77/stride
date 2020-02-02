// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Shaders.Ast;

using StorageQualifier = Xenko.Core.Shaders.Ast.Hlsl.StorageQualifier;

namespace Xenko.Core.Shaders.Ast.Xenko
{
    public static class XenkoStorageQualifier
    {
        /// <summary>
        ///   Stream keyword (stream).
        /// </summary>
        public static readonly Qualifier Stream = new Qualifier("stream");

        /// <summary>
        ///   Patch stream keyword (patchstream).
        /// </summary>
        public static readonly Qualifier PatchStream = new Qualifier("patchstream");

        /// <summary>
        ///   Stage keyword (stage).
        /// </summary>
        public static readonly Qualifier Stage = new Qualifier("stage");

        /// <summary>
        ///   Clone keyword (clone).
        /// </summary>
        public static readonly Qualifier Clone = new Qualifier("clone");

        /// <summary>
        ///   Override keyword (override).
        /// </summary>
        public static readonly Qualifier Override = new Qualifier("override");

        /// <summary>
        ///   Override keyword (override).
        /// </summary>
        public static readonly Qualifier Abstract = new Qualifier("abstract");

        /// <summary>
        ///   Compose keyword (compose).
        /// </summary>
        public static readonly Qualifier Compose = new Qualifier("compose");

        /// <summary>
        ///   Internal keyword (internal).
        /// </summary>
        public static readonly Qualifier Internal = new Qualifier("internal");

        /// <summary>
        /// Parses the specified enum name.
        /// </summary>
        /// <param name="enumName">
        /// Name of the enum.
        /// </param>
        /// <returns>
        /// A qualifier
        /// </returns>
        public static Qualifier Parse(string enumName)
        {
            if (enumName == (string)Stream.Key)
                return Stream;
            if (enumName == (string)PatchStream.Key)
                return PatchStream;
            if (enumName == (string)Stage.Key)
                return Stage;
            if (enumName == (string)Clone.Key)
                return Clone;
            if (enumName == (string)Override.Key)
                return Override;
            if (enumName == (string)Abstract.Key)
                return Abstract;
            if (enumName == (string)Compose.Key)
                return Compose;
            if (enumName == (string)Internal.Key)
                return Internal;

            // Fallback to shared parameter qualifiers
            return Ast.Hlsl.StorageQualifier.Parse(enumName);
        }
    }
}
