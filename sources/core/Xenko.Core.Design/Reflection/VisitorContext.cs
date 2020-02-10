// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Core.Reflection
{
    public struct VisitorContext
    {
        public IDataVisitor Visitor { get; set; }

        public ITypeDescriptorFactory DescriptorFactory { get; set; }

        public object Instance { get; set; }

        public ObjectDescriptor Descriptor { get; set; }
    }
}
