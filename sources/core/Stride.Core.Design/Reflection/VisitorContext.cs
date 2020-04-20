// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Core.Reflection
{
    public struct VisitorContext
    {
        public IDataVisitor Visitor { get; set; }

        public ITypeDescriptorFactory DescriptorFactory { get; set; }

        public object Instance { get; set; }

        public ObjectDescriptor Descriptor { get; set; }
    }
}
