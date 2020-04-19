// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;

using Xenko.Core.Annotations;

namespace Xenko.Assets.Presentation.CurveEditor.ViewModels
{
    using WindowsPoint = System.Windows.Point;

    internal static class CurveHelper
    {
        public static ControlPointViewModelBase GetClosestPoint([ItemNotNull, NotNull]  IEnumerable<ControlPointViewModelBase> points, WindowsPoint position, double maximumDistance = double.PositiveInfinity)
        {
            ControlPointViewModelBase closest = null;
            var closestDistance = double.MaxValue;
            foreach (var p in points)
            {
                var distance = (p.ActualPoint - position).LengthSquared;
                if (distance < closestDistance && distance < maximumDistance * maximumDistance)
                {
                    closest = p;
                    closestDistance = distance;
                }
            }
            return closest;
        }
    }
}
