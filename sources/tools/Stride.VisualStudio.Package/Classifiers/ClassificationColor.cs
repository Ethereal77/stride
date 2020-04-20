// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2014 F# Software Foundation
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Windows.Media;

namespace Stride.VisualStudio.Classifiers
{
    public struct ClassificationColor
    {
        public readonly Color? ForegroundColor;
        public readonly Color? BackgroundColor;

        public ClassificationColor(Color? foregroundColor = null, Color? backgroundColor = null)
        {
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
        }
    }
}
