// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Presentation.View
{
    /// <summary>
    /// A default implementation of the <see cref="ITemplateProvider"/> interface that matches any object.
    /// </summary>
    public class DefaultTemplateProvider : TemplateProviderBase
    {
        /// <inheritdoc/>
        public override string Name => "Default";

        /// <inheritdoc/>
        public override bool Match(object obj)
        {
            return true;
        }
    }
}
