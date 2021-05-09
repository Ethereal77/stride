// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Specialized;
using System.Linq;
using System.Windows;

using Stride.Core.Assets.Editor.ViewModel;
using Stride.Core.Presentation.Behaviors;

namespace Stride.Core.Assets.Editor.View.Behaviors
{
    public abstract class ActivateOnLocationChangedBehavior<T> : ActivateOnCollectionChangedBehavior<T> where T : DependencyObject
    {
        private bool selectionDone;

        protected override bool MatchChange(NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                selectionDone = false;
            }
            if (e.Action == NotifyCollectionChangedAction.Add && !selectionDone)
            {
                if (e.NewItems.OfType<DirectoryBaseViewModel>().Any())
                {
                    selectionDone = true;
                }
            }
            return selectionDone;
        }
    }
}
