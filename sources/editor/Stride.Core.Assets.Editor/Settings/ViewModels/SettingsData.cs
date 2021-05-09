// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Stride.Core;

namespace Stride.Core.Assets.Editor.Settings.ViewModels
{
    public class SettingsData
    {
        public const string HasAcceptableValues = nameof(HasAcceptableValues);
        public const string AcceptableValues = nameof(AcceptableValues);
        public static readonly PropertyKey<bool> HasAcceptableValuesKey = new PropertyKey<bool>(HasAcceptableValues, typeof(SettingsData));
        public static readonly PropertyKey<IEnumerable<object>> AcceptableValuesKey = new PropertyKey<IEnumerable<object>>(AcceptableValues, typeof(SettingsData));

    }
}
