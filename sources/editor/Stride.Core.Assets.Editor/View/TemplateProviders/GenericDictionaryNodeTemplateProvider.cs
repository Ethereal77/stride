// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Stride.Core.Presentation.Quantum;
using Stride.Core.Presentation.Quantum.View;
using Stride.Core.Presentation.Quantum.ViewModels;

namespace Stride.Core.Assets.Editor.View.TemplateProviders
{
    public class GenericDictionaryNodeTemplateProvider : NodeViewModelTemplateProvider
    {
        public override string Name => $"Dictionary<{KeyType?.Name ?? "[ANY]"},{ValueType?.Name ?? "[ANY]"}>";

        public Type KeyType { get; set; }

        public Type ValueType { get; set; }

        public override bool MatchNode(NodeViewModel node)
        {
            if (node.HasDictionary && node.NodeValue != null)
            {
                var dictionaryType = node.NodeValue.GetType();
                if (dictionaryType.IsGenericType)
                {
                    var genericTypes = dictionaryType.GetGenericArguments();
                    if (genericTypes.Length == 2)
                    {
                        return (KeyType == null || KeyType.IsAssignableFrom(genericTypes[0])) && (ValueType == null || ValueType.IsAssignableFrom(genericTypes[1]));
                    }
                }
            }
            return false;
        }
    }
}
