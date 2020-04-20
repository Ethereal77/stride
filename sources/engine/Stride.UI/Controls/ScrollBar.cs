// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Diagnostics;

using Stride.Core;
using Stride.Core.Mathematics;

namespace Stride.UI.Controls
{
    /// <summary>
    /// Represents a scroll bar.
    /// </summary>
    [DataContract(nameof(ScrollBar))]
    [DebuggerDisplay("ScrollBar - Name={Name}")]
    public class ScrollBar : UIElement
    {
        public ScrollBar()
        {
            BarColorInternal = Color.Transparent;
        }

        internal Color BarColorInternal;

        /// <summary>
        /// The color of the bar.
        /// </summary>
        /// <userdoc>The color of the bar.</userdoc>
        [DataMember]
        [Display(category: AppearanceCategory)]
        public Color BarColor
        {
            get { return BarColorInternal; }
            set { BarColorInternal = value; }
        }
    }
}
