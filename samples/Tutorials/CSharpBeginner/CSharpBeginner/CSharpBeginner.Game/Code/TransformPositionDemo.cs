// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Mathematics;
using Stride.Engine;

namespace CSharpBeginner.Code
{
    /// <summary>
    ///   This script demonstrates how to access the entity's local and world position and displays them on screen. 
    /// </summary>
    public class TransformPositionDemo : SyncScript
    {
        public override void Update()
        {
            // We store the local and world position of our entity's tranform in a Vector3 variable
            Vector3 localPosition = Entity.Transform.Position;
            Vector3 worldPosition = Entity.Transform.WorldMatrix.TranslationVector;

            // We display the entity's name and its local and world position on screen
            DebugText.Print(Entity.Name + " - local position: " + localPosition, new Int2(400, 450));
            DebugText.Print(Entity.Name + " - world position: " + worldPosition, new Int2(400, 470));
        }
    }
}
