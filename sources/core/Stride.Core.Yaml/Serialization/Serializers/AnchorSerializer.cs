// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;

using Xenko.Core.Yaml.Events;

namespace Xenko.Core.Yaml.Serialization.Serializers
{
    internal class AnchorSerializer : ChainedSerializer
    {
        private readonly Dictionary<string, object> aliasToObject;
        private readonly Dictionary<object, string> objectToAlias;

        public AnchorSerializer()
        {
            aliasToObject = new Dictionary<string, object>();
            objectToAlias = new Dictionary<object, string>(new IdentityEqualityComparer<object>());
        }

        public bool TryGetAliasValue(string alias, out object value)
        {
            return aliasToObject.TryGetValue(alias, out value);
        }

        public override object ReadYaml(ref ObjectContext objectContext)
        {
            var context = objectContext.SerializerContext;
            var reader = context.Reader;
            object value = null;

            // Process Anchor alias (*oxxx)
            var alias = reader.Allow<AnchorAlias>();
            if (alias != null)
            {
                // Return an alias or directly the value
                if (!aliasToObject.TryGetValue(alias.Value, out value))
                {
                    throw new AnchorNotFoundException(alias.Value, alias.Start, alias.End, "Unable to find alias");
                }
                return value;
            }

            // Test if current node has an anchor &oxxx
            string anchor = null;
            var nodeEvent = reader.Peek<NodeEvent>();
            if (nodeEvent != null && !string.IsNullOrEmpty(nodeEvent.Anchor))
            {
                anchor = nodeEvent.Anchor;
            }

            // Deserialize the current node
            value = base.ReadYaml(ref objectContext);

            // Store Anchor (&oxxx) and override any defined anchor 
            if (anchor != null)
            {
                aliasToObject[anchor] = value;
            }

            return value;
        }

        public override void WriteYaml(ref ObjectContext objectContext)
        {
            var value = objectContext.Instance;

            // Only write anchors for object (and not value types)
            bool isAnchorable = false;
            if (value != null && !value.GetType().IsValueType)
            {
                var typeCode = Type.GetTypeCode(value.GetType());
                switch (typeCode)
                {
                    case TypeCode.Object:
                    case TypeCode.String:
                        isAnchorable = true;
                        break;
                }
            }

            if (isAnchorable)
            {
                string alias;
                if (objectToAlias.TryGetValue(value, out alias))
                {
                    objectContext.Writer.Emit(new AliasEventInfo(value, value.GetType()) {Alias = alias});
                    return;
                }
                else
                {
                    alias = string.Format("o{0}", objectContext.SerializerContext.AnchorCount);
                    objectToAlias.Add(value, alias);

                    objectContext.Anchor = alias;
                    objectContext.SerializerContext.AnchorCount++;
                }
            }

            base.WriteYaml(ref objectContext);
        }
    }
}
