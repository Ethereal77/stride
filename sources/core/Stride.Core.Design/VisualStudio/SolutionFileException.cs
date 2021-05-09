// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2009 SLNTools - Christian Warren
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.VisualStudio
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
