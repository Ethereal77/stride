// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Core.Annotations;

namespace Xenko.Core.Presentation.Quantum.Presenters
{
    public class NodePresenterException : Exception
    {
        public NodePresenterException([NotNull] string message) : base(message)
        {
        }

        public NodePresenterException([NotNull] string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
