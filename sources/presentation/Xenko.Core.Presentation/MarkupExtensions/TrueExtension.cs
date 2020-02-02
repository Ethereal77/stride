// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Windows.Markup;

using Xenko.Core.Presentation.Internal;

namespace Xenko.Core.Presentation.MarkupExtensions
{
    [MarkupExtensionReturnType(typeof(bool))]
    public sealed class TrueExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return BooleanBoxes.TrueBox;
        }
    }
}
