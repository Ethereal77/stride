// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

using Stride.Core.Yaml.Serialization;

namespace Stride.Core.Yaml
{
    /// <summary>
    /// Dynamic version of <see cref="YamlSequenceNode"/>.
    /// </summary>
    public class DynamicYamlArray : DynamicYamlObject, IDynamicYamlNode, IEnumerable
    {
        internal YamlSequenceNode node;

        public YamlSequenceNode Node => node;

        public int Count => node.Children.Count;

        YamlNode IDynamicYamlNode.Node => Node;

        public DynamicYamlArray(YamlSequenceNode node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            this.node = node;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return node.Children.Select(ConvertToDynamic).ToArray().GetEnumerator();
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            if (binder.Type.IsAssignableFrom(node.GetType()))
            {
                result = node;
            }
            else
            {
                throw new InvalidOperationException();
            }
            return true;
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            var key = Convert.ToInt32(indexes[0]);
            node.Children[key] = ConvertFromDynamic(value);
            return true;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            var key = Convert.ToInt32(indexes[0]);
            result = ConvertToDynamic(node.Children[key]);
            return true;
        }

        public void Add(object value)
        {
            node.Children.Add(ConvertFromDynamic(value));
        }

        public void RemoveAt(int index)
        {
            node.Children.RemoveAt(index);
        }
    }
}
