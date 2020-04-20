// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.ComponentModel.Composition;

using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Stride.VisualStudio.Assets
{
	internal static class AssetObjectDefinitions
	{
        /// <summary>
        /// Content Type
        /// </summary>
        [Export]
        [Name(Constants.ContentType)]
        [BaseDefinition("code")]
        internal static ContentTypeDefinition hidingContentTypeDefinition = null;

        /// <summary>
        /// File extensions
        /// </summary>
		[Export]
		[FileExtension(".sdpkg")]
		[ContentType(Constants.ContentType)]
		internal static FileExtensionToContentTypeDefinition sdpkgFileExtensionDefinition = null;
		
		[Export]
        [FileExtension(".sdfnt")]
        [ContentType(Constants.ContentType)]
        internal static FileExtensionToContentTypeDefinition sdfntFileExtensionDefinition = null;

        [Export]
        [FileExtension(".sdfxlib")]
        [ContentType(Constants.ContentType)]
        internal static FileExtensionToContentTypeDefinition sdfxlibFileExtensionDefinition = null;

        [Export]
        [FileExtension(".sdlightconf")]
        [ContentType(Constants.ContentType)]
        internal static FileExtensionToContentTypeDefinition sdlightconfFileExtensionDefinition = null;

        [Export]
        [FileExtension(".sdtex")]
        [ContentType(Constants.ContentType)]
        internal static FileExtensionToContentTypeDefinition sdtexFileExtensionDefinition = null;

        [Export]
        [FileExtension(".sdscene")]
        [ContentType(Constants.ContentType)]
        internal static FileExtensionToContentTypeDefinition sdsceneFileExtensionDefinition = null;

	    [Export]
	    [FileExtension(".sdprefab")]
	    [ContentType(Constants.ContentType)]
	    internal static FileExtensionToContentTypeDefinition sdprefabFileExtensionDefinition = null;

        [Export]
        [FileExtension(".sdm3d")]
        [ContentType(Constants.ContentType)]
        internal static FileExtensionToContentTypeDefinition sdm3dFileExtensionDefinition = null;

        [Export]
        [FileExtension(".sdanim")]
        [ContentType(Constants.ContentType)]
        internal static FileExtensionToContentTypeDefinition sdanimFileExtensionDefinition = null;

        [Export]
        [FileExtension(".sdsnd")]
        [ContentType(Constants.ContentType)]
        internal static FileExtensionToContentTypeDefinition sdsndFileExtensionDefinition = null;

        [Export]
        [FileExtension(".sdmat")]
        [ContentType(Constants.ContentType)]
        internal static FileExtensionToContentTypeDefinition sdmatFileExtensionDefinition = null;

        [Export]
        [FileExtension(".sdsprite")]
        [ContentType(Constants.ContentType)]
        internal static FileExtensionToContentTypeDefinition sdsprtFileExtensionDefinition = null;

        /// <summary>
        /// Classification type definitions
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name(AnchorClassificationName)]
		internal static ClassificationTypeDefinition YamlAnchorType = null;
	    public const string AnchorClassificationName = "Stride.Yaml.Anchor";

		[Export(typeof(ClassificationTypeDefinition))]
        [Name(AliasClassificationName)]
		internal static ClassificationTypeDefinition YamlAliasType = null;
        public const string AliasClassificationName = "Stride.Yaml.Alias";

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(NumberClassificationName)]
        internal static ClassificationTypeDefinition YamlNumberType = null;
        public const string NumberClassificationName = "Stride.Yaml.Number";

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(KeyClassificationName)]
        internal static ClassificationTypeDefinition YamlKeyType = null;
        public const string KeyClassificationName = "Stride.Yaml.Key";

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(ErrorClassificationName)]
        internal static ClassificationTypeDefinition YamlErrorType = null;
        public const string ErrorClassificationName = "Stride.Yaml.Error";
    }
}
