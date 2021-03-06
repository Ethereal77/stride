// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Assets.Editor.ViewModel;
using Stride.Core;

namespace Stride.Core.Assets.Editor.Quantum.NodePresenters.Keys
{
    public static class SessionData
    {
        public const string Session = nameof(Session);
        public const string DynamicThumbnail = nameof(DynamicThumbnail);

        public static readonly PropertyKey<SessionViewModel> SessionKey = new PropertyKey<SessionViewModel>(Session, typeof(SessionData));
        public static readonly PropertyKey<bool> DynamicThumbnailKey = new PropertyKey<bool>(DynamicThumbnail, typeof(SessionData));
    }
}
