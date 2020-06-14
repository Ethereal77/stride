// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Linq;

namespace Stride.Core.Presentation.Services
{
    /// <summary>
    ///   Represents a filter for a file dialog.
    /// </summary>
    public struct FileDialogFilter
    {
        /// <summary>
        ///   Gets the description of this filter.
        /// </summary>
        public string Description { get; }
        /// <summary>
        ///   Gets the list of extensions for this filter, concatenated in a string.
        /// </summary>
        public string ExtensionList { get; }

        /// <summary>
        ///   Initializes a new <see cref="FileDialogFilter"/> structure.
        /// </summary>
        /// <param name="description">The description of this filter.</param>
        /// <param name="extensionList">The list of extensions for this filter, concatenated in a string.</param>
        public FileDialogFilter(string description, string extensionList)
        {
            Description = description;
            // Microsoft.WindowsAPICodePack.Shell doesn't seem to accept .ext anymore, only *.ext or ext
            ExtensionList = string.Join(";", extensionList.Split(';').Select(x => x.TrimStart('.')));
        }
    }
}
