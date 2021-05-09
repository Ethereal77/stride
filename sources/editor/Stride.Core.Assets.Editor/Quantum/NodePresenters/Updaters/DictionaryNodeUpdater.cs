// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using Stride.Core.Presentation.Quantum.Presenters;
using Stride.Core.Reflection;

namespace Stride.Core.Assets.Editor.Quantum.NodePresenters.Updaters
{
    /// <summary>
    /// Updates nodes that hold dictionaries with the dictionary's key type.
    /// </summary>
    public sealed class DictionaryNodeUpdater : NodePresenterUpdaterBase
    {
        public static PropertyKey<Type> DictionaryNodeKeyType = new PropertyKey<Type>(nameof(DictionaryNodeKeyType), typeof(DictionaryNodeUpdater));
        
        public override void UpdateNode(INodePresenter node)
        {
            if (DictionaryDescriptor.IsDictionary(node.Type))
            {
                if (node.Type.IsGenericType)
                {
                    var genericArguments = node.Type.GetGenericArguments();
                    node.AttachedProperties.Add(DictionaryNodeKeyType, genericArguments[0]);
                }
                else
                {
                    foreach (var typeInterface in node.Type.GetInterfaces())
                    {
                        if (!typeInterface.IsGenericType || typeInterface.GetGenericTypeDefinition() != typeof(IDictionary<,>))
                            continue;

                        var genericArguments = typeInterface.GetGenericArguments();
                        node.AttachedProperties.Add(DictionaryNodeKeyType, genericArguments[0]);
                        break;
                    }
                }
            }
        }
    }
}
