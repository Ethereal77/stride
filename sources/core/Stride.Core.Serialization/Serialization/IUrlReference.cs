// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Serialization
{
    /// <summary>
    ///   Represents an URL to an Asset.
    /// </summary>
    public interface IUrlReference
    {
        // <summary>
        ///   Gets the URL of the referenced Asset.
        /// </summary>
        string Url { get; }

        /// <summary>
        ///   Gets a value indicating whether the reference is <c>null</c> or empty.
        /// </summary>
        bool IsEmpty { get; }
    }
}
