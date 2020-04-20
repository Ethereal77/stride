// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.



namespace Stride.Particles.VertexLayouts
{
    public interface IAttributeTransformer<T, U>
    {
        void Transform(ref T attribute, ref U transformer);
    }
}
