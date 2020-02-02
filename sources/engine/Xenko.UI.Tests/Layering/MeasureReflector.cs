// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Mathematics;

namespace Xenko.UI.Tests.Layering
{
    /// <summary>
    /// Element that returns the size provided during the measure.
    /// Can be used to analyze the size during measure.
    /// </summary>
    public class MeasureReflector: UIElement
    {
        protected override Vector3 MeasureOverride(Vector3 availableSizeWithoutMargins)
        {
            return availableSizeWithoutMargins;
        }
    }
}
