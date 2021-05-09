// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Stride.Core;
using Stride.Core.Annotations;
using Stride.Core.Quantum;
using Stride.Assets.Rendering;
using Stride.Rendering.Compositing;

namespace Stride.Assets.Presentation.AssetEditors.GraphicsCompositorEditor.ViewModels
{
    public class SharedRendererBlockViewModel : SharedRendererBlockBaseViewModel
    {
        private readonly Dictionary<SharedRendererReferenceKey, IGraphicsCompositorSlotViewModel> outputSlotMap = new Dictionary<SharedRendererReferenceKey, IGraphicsCompositorSlotViewModel>();
        private readonly IObjectNode sharedRendererNode;
        private readonly ISharedRenderer sharedRenderer;

        public SharedRendererBlockViewModel([NotNull] GraphicsCompositorEditorViewModel editor, ISharedRenderer sharedRenderer) : base(editor)
        {
            this.sharedRenderer = sharedRenderer;
            sharedRendererNode = editor.Session.AssetNodeContainer.GetOrCreateNode(sharedRenderer);
            InputSlots.Add(new SharedRendererInputSlotViewModel(this));
        }

        /// <inheritdoc/>
        public override string Title => DisplayAttribute.GetDisplayName(sharedRenderer?.GetType());

        public ISharedRenderer GetSharedRenderer() => sharedRenderer;

        /// <inheritdoc/>
        protected override IEnumerable<IGraphNode> GetNodesContainingReferences()
        {
            yield return sharedRendererNode;
        }

        /// <inheritdoc/>
        public override IObjectNode GetRootNode() => sharedRendererNode;

        /// <inheritdoc />
        protected override GraphNodePath GetNodePath()
        {
            var path = new GraphNodePath(Editor.Session.AssetNodeContainer.GetNode(Editor.Asset.Asset));
            path.PushMember(nameof(GraphicsCompositorAsset.SharedRenderers));
            path.PushTarget();
            path.PushIndex(new NodeIndex(Editor.Asset.Asset.SharedRenderers.IndexOf(sharedRenderer)));
            return path;
        }
    }
}
