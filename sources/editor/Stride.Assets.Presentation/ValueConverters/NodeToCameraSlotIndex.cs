// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Stride.Core.Assets.Editor.Quantum.NodePresenters.Commands;
using Stride.Rendering.Compositing;
using Stride.Core.Presentation.ValueConverters;

namespace Stride.Assets.Presentation.ValueConverters
{
    public class NodeToCameraSlotIndex : OneWayMultiValueConverter<NodeToCameraSlotIndex>
    {
        /// <inheritdoc/>
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var slotIndex = (SceneCameraSlotId)values[0];

            var slots = values[1] as IList<AbstractNodeEntry>;
            if (slots != null)
            {
                foreach (var slot in slots.OfType<AbstractNodeValue>().Where(x => x.Value is SceneCameraSlotId))
                {
                    if (((SceneCameraSlotId)slot.Value).Id == slotIndex.Id)
                        return slot.DisplayValue;
                }
            }

            return "(Invalid camera index)";
        }
    }
}
