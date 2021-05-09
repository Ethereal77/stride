// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Assets;
using Stride.Core.Assets.Compiler;
using Stride.Engine;

namespace Stride.Assets.Entities.ComponentChecks
{
    /// <summary>
    ///   Defines the interface of Component checks executed during Asset Compilation.
    /// </summary>
    public interface IEntityComponentCheck
    {
        /// <summary>
        ///   Determines if a Component can be passed to <see cref="Check(EntityComponent)"/>.
        /// </summary>
        /// <param name="componentType">Type of the Component to be checked.</param>
        /// <returns>
        ///   <c>true</c> if the Component of type <paramref name="componentType"/> can be passed to <see cref="Check(EntityComponent)"/>;
        ///   <c>false</c> otherwise.
        /// </returns>
        bool AppliesTo(Type componentType);

        /// <summary>
        ///   Checks if the Component state is valid and reports appropriate errors / warnings if not.
        /// </summary>
        /// <param name="component">The Component to check.</param>
        /// <param name="entity">The Entity the <paramref name="component"/> is associated with.</param>
        /// <param name="assetItem">The Asset item the <paramref name="entity"/> belongs to.</param>
        /// <param name="targetUrlInStorage">The URL of the <paramref name="assetItem"/>.</param>
        /// <param name="result">The Asset Compilation result to report to.</param>
        void Check(EntityComponent component, Entity entity, AssetItem assetItem, string targetUrlInStorage, AssetCompilerResult result);
    }
}
