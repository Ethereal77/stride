// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Engine;

namespace Stride.Physics
{
    /// <summary>
    /// Extension methods for the <see cref="ScriptComponent"/> related to phystics
    /// </summary>
    public static class PhysicsScriptComponentExtensions
    {
        /// <summary>
        /// Gets the curent <see cref="Simulation"/>.
        /// </summary>
        /// <param name="scriptComponent">The script component to query physics from</param>
        /// <returns>The simulation object or null if there are no simulation running for the current scene.</returns>
        public static Simulation GetSimulation(this ScriptComponent scriptComponent)
        {
            return scriptComponent.SceneSystem.SceneInstance.GetProcessor<PhysicsProcessor>()?.Simulation;
        }
    }
}
