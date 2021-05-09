// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2014 F# Software Foundation
// See the LICENSE.md file in the project root for full license information.

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
