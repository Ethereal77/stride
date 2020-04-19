// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;
using Xenko.Core.Collections;

namespace Xenko.Rendering.Images
{
    /// <summary>
    /// A collection of <see cref="ColorTransformBase"/>
    /// </summary>
    [DataContract("ColorTransformCollection")]
    public class ColorTransformCollection : SafeList<ColorTransform>
    {
        public T Get<T>() where T : ColorTransform
        {
            foreach (var transform in this)
            {
                if (typeof(T) == transform.GetType())
                {
                    return (T)transform;
                }
            }
            return null;
        }

        public bool IsEnabled<T>() where T : ColorTransform
        {
            foreach (var transform in this)
            {
                if (typeof(T) == transform.GetType())
                {
                    return transform.Enabled;
                }
            }
            return false;
        }
    }
}
