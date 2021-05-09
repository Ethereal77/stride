// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Collections;

namespace Stride.Rendering
{
    public class ParameterCollectionLayout
    {
        public FastListStruct<ParameterKeyInfo> LayoutParameterKeyInfos = new FastListStruct<ParameterKeyInfo>(0);
        public int ResourceCount;
        public int BufferSize;
    }
}
