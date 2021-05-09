// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;

namespace Stride.Core.Assets.Editor.Quantum.NodePresenters.Keys
{
    public static class DisplayData
    {
        public const string AttributeDisplayName = nameof(AttributeDisplayName);
        public const string AutoExpandRule = nameof(AutoExpandRule);
        public const string UnloadableObjectInfo = nameof(UnloadableObjectInfo);

        public static readonly PropertyKey<string> AttributeDisplayNameKey = new PropertyKey<string>(AttributeDisplayName, typeof(DisplayData));
        public static readonly PropertyKey<ExpandRule> AutoExpandRuleKey = new PropertyKey<ExpandRule>(AutoExpandRule, typeof(DisplayData));
    }
}
