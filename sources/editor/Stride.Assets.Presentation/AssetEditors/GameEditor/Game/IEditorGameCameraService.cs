// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Mathematics;
using Stride.Editor.EditorGame.Game;
using Stride.Engine;

namespace Stride.Assets.Presentation.AssetEditors.GameEditor.Game
{
    public interface IEditorGameCameraService : IEditorGameService
    {
        CameraComponent Component { get; }

        Matrix ViewMatrix { get; }

        Matrix ProjectionMatrix { get; }

        Vector3 Position { get; }

        bool IsMoving { get; }

        bool IsOrthographic { get; set; }

        float AspectRatio { get; }

        float VerticalFieldOfView { get; }

        float NearPlane { get; }

        float FarPlane { get; }

        float SceneUnit { get; }

        float MoveSpeed { get; }

        void ResetCamera(Vector3 viewDirection);

        void ResetCamera(CameraOrientation orientation);
    }
}
