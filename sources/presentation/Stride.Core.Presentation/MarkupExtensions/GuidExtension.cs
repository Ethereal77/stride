// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Windows.Markup;

using Stride.Core.Annotations;

namespace Stride.Core.Presentation.MarkupExtensions
{
    [MarkupExtensionReturnType(typeof(Guid))]
    public sealed class GuidExtension : MarkupExtension
    {
        public Guid Value { get; set; }

        public GuidExtension()
        {
            Value = Guid.Empty;
        }

        public GuidExtension(object value)
        {
            Guid guid;
            Guid.TryParse(value as string, out guid);
            Value = guid;
        }

        /// <inheritdoc/>
        [NotNull]
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Value;
        }
    }
}
