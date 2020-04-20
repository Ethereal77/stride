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
		[FileExtension(".xkpkg")]
		[ContentType(Constants.ContentType)]
		internal static FileExtensionToContentTypeDefinition xkpkgFileExtensionDefinition = null;
		
		[Export]
        [FileExtension(".xkfnt")]
        [ContentType(Constants.ContentType)]
        internal static FileExtensionToContentTypeDefinition xkfntFileExtensionDefinition = null;

        [Export]
        [FileExtension(".xkfxlib")]
        [ContentType(Constants.ContentType)]
        internal static FileExtensionToContentTypeDefinition xkfxlibFileExtensionDefinition = null;

        [Export]
        [FileExtension(".xklightconf")]
        [ContentType(Constants.ContentType)]
        internal static FileExtensionToContentTypeDefinition xklightconfFileExtensionDefinition = null;

        [Export]
        [FileExtension(".xktex")]
        [ContentType(Constants.ContentType)]
        internal static FileExtensionToContentTypeDefinition xktexFileExtensionDefinition = null;

        [Export]
        [FileExtension(".xkscene")]
        [ContentType(Constants.ContentType)]
        internal static FileExtensionToContentTypeDefinition xksceneFileExtensionDefinition = null;

	    [Export]
	    [FileExtension(".xkprefab")]
	    [ContentType(Constants.ContentType)]
	    internal static FileExtensionToContentTypeDefinition xkprefabFileExtensionDefinition = null;

        [Export]
        [FileExtension(".xkm3d")]
        [ContentType(Constants.ContentType)]
        internal static FileExtensionToContentTypeDefinition xkm3dFileExtensionDefinition = null;

        [Export]
        [FileExtension(".xkanim")]
        [ContentType(Constants.ContentType)]
        internal static FileExtensionToContentTypeDefinition xkanimFileExtensionDefinition = null;

        [Export]
        [FileExtension(".xksnd")]
        [ContentType(Constants.ContentType)]
        internal static FileExtensionToContentTypeDefinition xksndFileExtensionDefinition = null;

        [Export]
        [FileExtension(".xkmat")]
        [ContentType(Constants.ContentType)]
        internal static FileExtensionToContentTypeDefinition xkmatFileExtensionDefinition = null;

        [Export]
        [FileExtension(".xksprite")]
        [ContentType(Constants.ContentType)]
        internal static FileExtensionToContentTypeDefinition xksprtFileExtensionDefinition = null;

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
