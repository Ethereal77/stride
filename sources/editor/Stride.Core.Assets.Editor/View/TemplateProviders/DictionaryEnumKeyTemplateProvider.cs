// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using Stride.Core.Assets.Editor.Quantum.NodePresenters.Updaters;
using Stride.Core.Presentation.Quantum;
using Stride.Core.Presentation.Quantum.ViewModels;

namespace Stride.Core.Assets.Editor.View.TemplateProviders
{
    public class DictionaryEnumKeyTemplateProvider : DictionaryTemplateProvider
    {
        public override string Name => "DictionaryEnumKey";

        /// <summary>
        /// If set to true, this provider will accept nodes representing entries of a enum-keyed dictionary.
        /// Otherwise, it will accept nodes representing the enum-keyed dictionary itself.
        /// </summary>
        public bool ApplyForItems { get; set; }

        public override bool MatchNode(NodeViewModel node)
        {
            if (ApplyForItems)
            {
                node = node.Parent;
                if (node == null)
                    return false;
            }

            if (!base.MatchNode(node))
                return false;

            if(node.AssociatedData.TryGetValue(DictionaryNodeUpdater.DictionaryNodeKeyType.Name, out var value))
            {
                var type = (Type)value;
                return type.IsEnum;
            }

            return false;
        }
    }
}
