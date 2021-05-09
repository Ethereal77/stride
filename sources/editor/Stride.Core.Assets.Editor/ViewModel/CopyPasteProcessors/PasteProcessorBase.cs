// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Threading.Tasks;

using Stride.Core.Assets.Editor.Services;
using Stride.Core.Assets.Quantum;
using Stride.Core.Assets.Yaml;
using Stride.Core;
using Stride.Core.Reflection;
using Stride.Core.Quantum;

namespace Stride.Core.Assets.Editor.ViewModel.CopyPasteProcessors
{
    public abstract class PasteProcessorBase : IPasteProcessor
    {
        /// <inheritdoc />
        public abstract bool Accept(Type targetRootType, Type targetMemberType, Type pastedDataType);

        /// <inheritdoc />
        public abstract bool ProcessDeserializedData(AssetPropertyGraphContainer graphContainer, object targetRootObject, Type targetMemberType, ref object data, bool isRootDataObjectReference, AssetId? sourceId, YamlAssetMetadata<OverrideType> overrides, YamlAssetPath basePath);

        /// <inheritdoc />
        public virtual Task Paste(IPasteItem pasteResultItem, AssetPropertyGraph propertyGraph, ref NodeAccessor nodeAccessor, ref PropertyContainer propertyContainer)
        {
            // default implementation does nothing
            return Task.CompletedTask;
        }
    }
}
