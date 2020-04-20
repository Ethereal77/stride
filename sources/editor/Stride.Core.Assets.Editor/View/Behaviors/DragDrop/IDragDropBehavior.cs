// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Core.Assets.Editor.View.Behaviors
{
    public interface IDragDropBehavior
    {
        bool CanDrag { get; set; }

        bool CanDrop { get; set; }

        DisplayDropAdorner DisplayDropAdorner { get; set; }
    }
}
