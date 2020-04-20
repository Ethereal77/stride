// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.ComponentModel.Composition;

using Microsoft.VisualStudio.Language.StandardClassification;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Stride.VisualStudio.Assets
{
	#region Format definition
	[Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = AssetObjectDefinitions.AnchorClassificationName)]
	[Name(AssetObjectDefinitions.AnchorClassificationName)]
	[UserVisible(true)] //this should be visible to the end user
	[Order(Before = Priority.Default)] //set the priority to be after the default classifiers
    [BaseDefinition(PredefinedClassificationTypeNames.SymbolReference)]
	internal sealed class YamlAnchorFormat : ClassificationFormatDefinition
	{
        [ImportingConstructor]
        public YamlAnchorFormat(AssetObjectClassificationColorManager colorManager)
		{
			DisplayName = "Stride YAML Anchor"; //human readable version of the name
            var classificationColor = colorManager.GetClassificationColor(AssetObjectDefinitions.AnchorClassificationName);
            ForegroundColor = classificationColor.ForegroundColor;
            BackgroundColor = classificationColor.BackgroundColor;
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = AssetObjectDefinitions.AliasClassificationName)]
    [Name(AssetObjectDefinitions.AliasClassificationName)]
    [UserVisible(true)] //this should be visible to the end user
	[Order(Before = Priority.Default)] //set the priority to be after the default classifiers
    [BaseDefinition(PredefinedClassificationTypeNames.Literal)]
	internal sealed class YamlAliasFormat : ClassificationFormatDefinition
	{
        [ImportingConstructor]
        public YamlAliasFormat(AssetObjectClassificationColorManager colorManager)
		{
            DisplayName = "Stride YAML Alias"; //human readable version of the name
            var classificationColor = colorManager.GetClassificationColor(AssetObjectDefinitions.AliasClassificationName);
            ForegroundColor = classificationColor.ForegroundColor;
            BackgroundColor = classificationColor.BackgroundColor;
        }
	}

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = AssetObjectDefinitions.KeyClassificationName)]
    [Name(AssetObjectDefinitions.KeyClassificationName)]
    [UserVisible(true)] //this should be visible to the end user
    [Order(Before = Priority.Default)] //set the priority to be after the default classifiers
    [BaseDefinition(PredefinedClassificationTypeNames.Keyword)]
    internal sealed class YamlKeyFormat : ClassificationFormatDefinition
    {
        [ImportingConstructor]
        public YamlKeyFormat(AssetObjectClassificationColorManager colorManager)
        {
            DisplayName = "Stride YAML Key"; //human readable version of the name
            var classificationColor = colorManager.GetClassificationColor(AssetObjectDefinitions.KeyClassificationName);
            ForegroundColor = classificationColor.ForegroundColor;
            BackgroundColor = classificationColor.BackgroundColor;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = AssetObjectDefinitions.NumberClassificationName)]
    [Name(AssetObjectDefinitions.NumberClassificationName)]
    [UserVisible(true)] //this should be visible to the end user
    [Order(Before = Priority.Default)] //set the priority to be after the default classifiers
    [BaseDefinition(PredefinedClassificationTypeNames.Number)]
    internal sealed class YamlNumberFormat : ClassificationFormatDefinition
    {
        [ImportingConstructor]
        public YamlNumberFormat(AssetObjectClassificationColorManager colorManager)
        {
            DisplayName = "Stride YAML Number"; //human readable version of the name
            var classificationColor = colorManager.GetClassificationColor(AssetObjectDefinitions.NumberClassificationName);
            ForegroundColor = classificationColor.ForegroundColor;
            BackgroundColor = classificationColor.BackgroundColor;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = AssetObjectDefinitions.ErrorClassificationName)]
    [Name(AssetObjectDefinitions.ErrorClassificationName)]
    [UserVisible(true)] //this should be visible to the end user
    [Order(Before = Priority.Default)] //set the priority to be after the default classifiers
    [BaseDefinition(PredefinedClassificationTypeNames.Other)]
    internal sealed class YamlErrorFormat : ClassificationFormatDefinition
    {
        [ImportingConstructor]
        public YamlErrorFormat(AssetObjectClassificationColorManager colorManager)
        {
            DisplayName = "Stride YAML Error"; //human readable version of the name
            var classificationColor = colorManager.GetClassificationColor(AssetObjectDefinitions.ErrorClassificationName);
            ForegroundColor = classificationColor.ForegroundColor;
            BackgroundColor = classificationColor.BackgroundColor;
        }
    }
	#endregion //Format definition
}
