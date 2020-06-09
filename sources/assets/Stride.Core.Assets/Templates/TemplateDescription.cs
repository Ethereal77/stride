// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

using Stride.Core.Annotations;
using Stride.Core.IO;

namespace Stride.Core.Assets.Templates
{
    /// <summary>
    ///   Describes a template generator that can be displayed in Game Studio.
    /// </summary>
    [DataContract("Template")]
    [NonIdentifiableCollectionItems]
    [DebuggerDisplay("Id: {Id}, Name: {Name}")]
    public class TemplateDescription : IFileSynchronizable
    {
        /// <summary>
        ///   The file extension used when loading/saving this template description.
        /// </summary>
        public const string FileExtension = ".sdtpl";

        /// <summary>
        ///   Gets or sets the unique identifier of the template.
        /// </summary>
        /// <value>The unique identifier.</value>
        [DataMember(0)]
        public Guid Id { get; set; }

        /// <summary>
        ///   Gets or sets the short name of the template.
        /// </summary>
        /// <value>The name.</value>
        [DataMember(10)]
        public string Name { get; set; }

        /// <summary>
        ///   Gets or sets the scope of the template.
        /// </summary>
        /// <value>The scope and context of the template.</value>
        [DataMember(15)]
        [DefaultValue(TemplateScope.Session)]
        public TemplateScope Scope { get; set; }

        /// <summary>
        ///   Gets or sets the order of the template.
        /// </summary>
        /// <value>
        ///   An integer that determines in which order are the template presented with respect to other templates in a list.
        ///   Lower value means higher order.
        /// </value>
        [DataMember(17)]
        [DefaultValue(0)]
        public int Order { get; set; }

        /// <summary>
        ///   Gets or sets the group name.
        /// </summary>
        /// <value>The group name inside which to present the template.</value>
        [DataMember(20)]
        [DefaultValue(null)]
        public string Group { get; set; }

        /// <summary>
        ///   Gets or sets the icon.
        /// </summary>
        /// <value>The filename of the icon that represents the template.</value>
        [DataMember(30)]
        [DefaultValue(null)]
        public UFile Icon { get; set; }

        /// <summary>
        ///   Gets a list of screenshots to show for the template.
        /// </summary>
        /// <value>A list of filenames of screenshots to show what the template is.</value>
        [DataMember(30)]
        public List<UFile> Screenshots { get; private set; } = new List<UFile>();

        /// <summary>
        ///   Gets or sets the short description of the template.
        /// </summary>
        /// <value>A short description of what the template is.</value>
        [DataMember(40)]
        [DefaultValue(null)]
        public string Description { get; set; }

        /// <summary>
        ///   Gets or sets the longer description of the template.
        /// </summary>
        /// <value>A more detailed description of what the template is.</value>
        [DataMember(43)]
        [DefaultValue(null)]
        public string FullDescription { get; set; }

        /// <summary>
        ///   Gets or set the default name for the output package/library.
        /// </summary>
        /// <value>The default output name.</value>
        [DataMember(45)]
        public string DefaultOutputName { get; set; }

        /// <summary>
        ///   Gets or sets the status of the template.
        /// </summary>
        /// <value>The status of the template.</value>
        [DataMember(60)]
        [DefaultValue(TemplateStatus.None)]
        public TemplateStatus Status { get; set; }

        /// <inheritdoc/>
        [DataMemberIgnore]
        public bool IsDirty { get; set; }

        /// <inheritdoc/>
        [DataMemberIgnore]
        public UFile FullPath { get; set; }

        /// <summary>
        ///   Gets the directory from where this template was loaded.
        /// </summary>
        /// <value>The resource directory.</value>
        [DataMemberIgnore]
        public UDirectory TemplateDirectory => FullPath?.GetParent();
    }
}
