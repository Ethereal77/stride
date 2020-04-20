// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Stride.Core.Annotations;

namespace Stride.Core.Presentation.View
{
    /// <summary>
    /// A default implementation of the <see cref="TemplateProviderComparerBase"/> class that compares <see cref="ITemplateProvider"/> instances by name.
    /// </summary>
    public class DefaultTemplateProviderComparer : TemplateProviderComparerBase
    {
        protected override int CompareProviders([NotNull] ITemplateProvider x, [NotNull] ITemplateProvider y)
        {
            return string.Compare(x.Name, y.Name, StringComparison.Ordinal);
        }
    }
}
