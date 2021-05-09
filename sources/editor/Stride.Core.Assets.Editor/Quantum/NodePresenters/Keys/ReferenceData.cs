// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Assets.Editor.ViewModel;
using Stride.Core;

namespace Stride.Core.Assets.Editor.Quantum.NodePresenters.Keys
{
    public static class ReferenceData
    {       
        public const string AddReferenceViewModel = nameof(AddReferenceViewModel);

        public static readonly PropertyKey<IAddReferenceViewModel> Key = new PropertyKey<IAddReferenceViewModel>(AddReferenceViewModel, typeof(ReferenceData));
    }
}
