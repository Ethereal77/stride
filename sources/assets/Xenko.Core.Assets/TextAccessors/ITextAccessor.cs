// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.IO;
using System.Threading.Tasks;

namespace Xenko.Core.Assets.TextAccessors
{
    public interface ITextAccessor
    {
        /// <summary>
        /// Gets the underlying text.
        /// </summary>
        /// <returns></returns>
        string Get();

        /// <summary>
        /// Sets the underlying text.
        /// </summary>
        /// <param name="value"></param>
        void Set(string value);

        /// <summary>
        /// Writes the text to the given <see cref="StreamWriter"/>.
        /// </summary>
        /// <param name="streamWriter"></param>
        Task Save(Stream streamWriter);

        ISerializableTextAccessor GetSerializableVersion();
    }
}
