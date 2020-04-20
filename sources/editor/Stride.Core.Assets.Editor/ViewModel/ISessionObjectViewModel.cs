// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core.Assets.Editor.Services;

namespace Stride.Core.Assets.Editor.ViewModel
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
 
