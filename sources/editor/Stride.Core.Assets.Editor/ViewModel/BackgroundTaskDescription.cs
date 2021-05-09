// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Data;

namespace Stride.Core.Assets.Editor.ViewModel
{
    public abstract class BackgroundTaskDescription : DependencyObject, IDisposable
    {
        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(BackgroundTaskDescription));

        public string Description { get { return (string)GetValue(DescriptionProperty); } set { SetValue(DescriptionProperty, value); } }

        public void Dispose()
        {
            BindingOperations.ClearAllBindings(this);
        }

        protected static void PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var description = (BackgroundTaskDescription)d;
            description.Description = description.UpdateDescription();
        }

        protected abstract string UpdateDescription();
    }
}
