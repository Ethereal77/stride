// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Core.Packages
{
    /// <summary>
    /// Possible level of logging used by <see cref="IPackagesLogger"/>.
    /// </summary>
    public enum MessageLevel
    {
        Debug,
        Verbose,
        Info,
        Minimal,
        Warning,
        Error,
        InfoSummary,
        ErrorSummary
    }
}
