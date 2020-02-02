// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Threading.Tasks;

using Xenko.Assets.Scripts;

namespace Xenko.Assets.Presentation.AssetEditors.VisualScriptEditor
{
    public interface IVisualScriptViewModelService
    {
        /// <summary>
        /// When a variable is dropped in the main view, we want to ask user what he wants to do with it (create a getter or setter?) and position it properly.
        /// </summary>
        /// <returns>The created block, or null if cancelled.</returns>
        Task<Block> TransformVariableIntoBlock(Symbol symbol);
    }
}
