// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;

namespace Xenko.Core.Assets.Templates
{
    /// <summary>
    /// Describes if a template is supporting a particular context
    /// </summary>
    [DataContract("TemplateScope")]
    public enum TemplateScope
    {
        // TODO We could use flags instead

        /// <summary>
        /// The template can be applied to an existing <see cref="PackageSession"/>.
        /// </summary>
        Session,

        /// <summary>
        /// The template can be applied to an existing <see cref="Assets.Package"/>.
        /// </summary>
        Package,

        /// <summary>
        /// The template can be applied to certain types of Assets <see cref="Assets.Asset"/>.
        /// </summary>
        Asset
    }
}
