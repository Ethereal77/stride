// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Assets.Editor.Services;

namespace Xenko.Core.Assets.Editor.ViewModel
{
    /// <summary>
    /// Interface for session objects.
    /// </summary>
    public interface ISessionObjectViewModel
    {
        bool IsEditable { get; }

        string Name { get; set; }

        SessionViewModel Session { get; }

        ThumbnailData ThumbnailData { get; }

        string TypeDisplayName { get; }
    }
}
 
