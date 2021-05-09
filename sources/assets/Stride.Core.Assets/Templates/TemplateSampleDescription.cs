// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.ComponentModel;

using Stride.Core;

namespace Stride.Core.Assets.Templates
{
    /// <summary>
    /// A template for using an existing package as a template, expecting a <see cref="Package"/> to be accessible 
    /// from <see cref="TemplateDescription.FullPath"/> with the same name as this template.
    /// </summary>
    [DataContract("TemplateSample")]
    public class TemplateSampleDescription : TemplateDescription
    {
        /// <summary>
        /// Gets or sets the name of the pattern used to substitute files and content. If null, use the 
        /// <see cref="TemplateDescription.DefaultOutputName"/>.
        /// </summary>
        /// <value>The name of the pattern.</value>
        [DataMember(70)]
        [DefaultValue(null)]
        public string PatternName { get; set; }
    }
}
