// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;

using Xenko.Core;
using Xenko.Rendering.Compositing;

namespace Xenko.Assets.Presentation.NodePresenters.Keys
{
    public static class CameraSlotData
    {
        public const string SceneCameraSlots = nameof(SceneCameraSlots);
        public const string UpdateCameraSlotIndex = nameof(UpdateCameraSlotIndex);

        public static readonly PropertyKey<List<SceneCameraSlot>> CameraSlotsKey = new PropertyKey<List<SceneCameraSlot>>(SceneCameraSlots, typeof(CameraSlotData));
    }
}
