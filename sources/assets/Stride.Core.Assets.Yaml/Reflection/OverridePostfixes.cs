// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Reflection
{
    public static class OverridePostfixes
    {
        internal const char PostFixSealed = '!';

        internal const char PostFixNew = '*';

        internal const string PostFixNewSealed = "*!";

        internal const string PostFixNewSealedAlt = "!*";

        internal const string PostFixSealedText = "!";

        internal const string PostFixNewText = "*";

        public static string ToText(this OverrideType type)
        {
            if (type == OverrideType.New)
            {
                return PostFixNewText;
            }
            if (type == OverrideType.Sealed)
            {
                return PostFixSealedText;
            }
            if (type == (OverrideType.New | OverrideType.Sealed))
            {
                return PostFixNewSealed;
            }
            return string.Empty;
        }
    }
}
