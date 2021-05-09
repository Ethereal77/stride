// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Presentation.Quantum.ViewModels;
using Stride.Core.Assets.Editor.Quantum.NodePresenters.Updaters;

namespace Stride.Core.Assets.Editor.View.TemplateProviders
{
    public class DictionaryStringKeyTemplateProvider : DictionaryTemplateProvider
    {
        public override string Name => "DictionaryStringKey";

        /// <summary>
        ///   Gets or sets a value indicating whether this provider should accept nodes representing entries
        ///   of a string-keyed dictionary.
        /// </summary>
        /// <value>
        ///   <c>true</c> to accept nodes representing entries of a string-keyed dictionary;
        ///   <c>false</c> to accept nodes representing the string-keyed dictionary itself.
        /// </value>
        public bool ApplyForItems { get; set; }

        public override bool MatchNode(NodeViewModel node)
        {
            if (ApplyForItems)
            {
                node = node.Parent;
                if (node is null)
                    return false;
            }

            if (!base.MatchNode(node))
                return false;

            if (node.AssociatedData.TryGetValue(DictionaryNodeUpdater.DictionaryNodeKeyType.Name, out var value))
            {
                var type = (Type) value;
                return type == typeof(string);
            }

            return false;
        }
    }
}
