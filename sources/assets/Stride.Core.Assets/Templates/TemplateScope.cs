// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Assets.Templates
{
    /// <summary>
    ///   Describes if a template is supporting a particular context.
    /// </summary>
    [DataContract("TemplateScope")]
    public enum TemplateScope
    {
        // TODO: We could use flags instead

        /// <summary>
        ///   The template can be applied to an existing <see cref="PackageSession"/>.
        /// </summary>
        Session,

        /// <summary>
        ///   The template can be applied to an existing <see cref="Assets.Package"/>.
        /// </summary>
        Package,

        /// <summary>
        ///   The template can be applied to certain types of <see cref="Assets.Asset"/>s.
        /// </summary>
        Asset
    }
}
