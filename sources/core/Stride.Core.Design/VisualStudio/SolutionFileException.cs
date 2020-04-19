// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2009 SLNTools - Christian Warren
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Core.VisualStudio
{
    class SolutionFileException : Exception
    {
        public SolutionFileException(string message)
            : base(message)
        {
        }

        public SolutionFileException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
