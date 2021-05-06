// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;

using Stride.Engine;

namespace Stride.Debugger.Target
{
    /// <summary>
    ///   Defines the interface that allows the debugger to control a game execution host, that can load and unload assemblies,
    ///   run games and update assets.
    /// </summary>
    public interface IGameDebuggerTarget
    {
        // == Target =============================================================================================== //

        void Exit();

        // == Assemblies =========================================================================================== //

        /// <summary>
        ///   Loads an assembly from disk.
        /// </summary>
        /// <param name="assemblyPath">The assembly path.</param>
        /// <returns>A <see cref="DebugAssembly"/> that represents an Assembly loaded in the debug process.</returns>
        DebugAssembly AssemblyLoad(string assemblyPath);

        /// <summary>
        ///   Loads an assembly directly from a memory buffer.
        /// </summary>
        /// <param name="peData">The PE data.</param>
        /// <param name="pdbData">The PDB data.</param>
        /// <returns>A <see cref="DebugAssembly"/> that represents an Assembly loaded in the debug process.</returns>
        DebugAssembly AssemblyLoadRaw(byte[] peData, byte[] pdbData);

        /// <summary>
        ///   Registers and unregisters a group of coherent assemblies.
        /// </summary>
        /// <param name="assembliesToUnregister">The assemblies to unregister.</param>
        /// <param name="assembliesToRegister">The assemblies to register.</param>
        /// <returns></returns>
        bool AssemblyUpdate(List<DebugAssembly> assembliesToUnregister, List<DebugAssembly> assembliesToRegister);

        // == Game ================================================================================================= //

        /// <summary>
        ///   Enumerates the game types available in the currently loaded assemblies.
        /// </summary>
        /// <returns>A <see cref="List{}"/> with the names of the types available in the currently loaded assemblies.</returns>
        List<string> GameEnumerateTypeNames();

        /// <summary>
        ///   Instantiates and launches a game specified by its type name.
        /// </summary>
        /// <param name="gameTypeName">Name of the <see cref="Game"/> type.</param>
        void GameLaunch(string gameTypeName);

        /// <summary>
        ///   Stops the current game, invoking <see cref="Game.Exit"/>.
        /// </summary>
        void GameStop();
    }
}
