// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Editor.EditorGame.ContentLoader
{
    public class ContentLoadEventArgs : EventArgs
    {
        public ContentLoadEventArgs(int contentLoadingCount)
        {
            ContentLoadingCount = contentLoadingCount;
        }

        public int ContentLoadingCount { get; }
    }
}
