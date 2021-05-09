// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Rendering;
using Stride.Shaders;

namespace Stride.Graphics
{
    public static class ParameterCollectionLayoutExtensions
    {
        public static void ProcessResources(this ParameterCollectionLayout parameterCollectionLayout, DescriptorSetLayoutBuilder layout)
        {
            foreach (var layoutEntry in layout.Entries)
            {
                parameterCollectionLayout.LayoutParameterKeyInfos.Add(new ParameterKeyInfo(layoutEntry.Key, parameterCollectionLayout.ResourceCount++));
            }
        }

        public static void ProcessConstantBuffer(this ParameterCollectionLayout parameterCollectionLayout, EffectConstantBufferDescription constantBuffer)
        {
            foreach (var member in constantBuffer.Members)
            {
                parameterCollectionLayout.LayoutParameterKeyInfos.Add(new ParameterKeyInfo(member.KeyInfo.Key, parameterCollectionLayout.BufferSize + member.Offset, member.Type.Elements > 0 ? member.Type.Elements : 1));
            }
            parameterCollectionLayout.BufferSize += constantBuffer.Size;
        }
    }
}
