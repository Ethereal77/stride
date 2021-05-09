// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2014 OxyPlot contributors
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Assets.Presentation.CurveEditor
{
    /// <summary>
    /// Defines change types for the <see cref="AxisBase.AxisChanged" /> event.
    /// </summary>
    public enum AxisChangeTypes
    {
        /// <summary>
        /// The axis was zoomed by the user.
        /// </summary>
        Zoom,

        /// <summary>
        /// The axis was panned by the user.
        /// </summary>
        Pan,

        /// <summary>
        /// The axis zoom/pan was reset by the user.
        /// </summary>
        Reset
    }

    /// <summary>
    /// Provides additional data for the <see cref="AxisBase.AxisChanged" /> event.
    /// </summary>
    public class AxisChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AxisChangedEventArgs" /> class.
        /// </summary>
        /// <param name="changeType">Type of the change.</param>
        /// <param name="deltaMinimum">The delta minimum.</param>
        /// <param name="deltaMaximum">The delta maximum.</param>
        public AxisChangedEventArgs(AxisChangeTypes changeType, double deltaMinimum, double deltaMaximum)
        {
            ChangeType = changeType;
            DeltaMinimum = deltaMinimum;
            DeltaMaximum = deltaMaximum;
        }

        /// <summary>
        /// Gets the type of the change.
        /// </summary>
        /// <value>The type of the change.</value>
        public AxisChangeTypes ChangeType { get; private set; }

        /// <summary>
        /// Gets the delta for the minimum.
        /// </summary>
        /// <value>The delta.</value>
        public double DeltaMinimum { get; private set; }

        /// <summary>
        /// Gets the delta for the maximum.
        /// </summary>
        /// <value>The delta.</value>
        public double DeltaMaximum { get; private set; }
    }
}
