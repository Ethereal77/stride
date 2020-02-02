// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Windows;
using System.Windows.Data;

namespace Xenko.Core.Assets.Editor.ViewModel
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
