// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.UI
{
    /// <summary>
    /// The exception that is thrown when an internal error happened in the UI System. That is an error that is not due to the user behavior.
    /// </summary>
    public class UIInternalException : Exception
    {
        internal UIInternalException(string msg)
            : base("An internal error happened in the UI system [details:'" + msg + "'")
        { }
    }
}
