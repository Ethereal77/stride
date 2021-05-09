// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Data;

using Stride.Core.Presentation.Graph.Behaviors;

namespace Stride.Core.Presentation.Graph.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class ConnectionWrapper : DependencyObject
    {
        public static DependencyProperty BindingProperty = DependencyProperty.Register(
            "Binding", 
            typeof(Binding),
            typeof(NodeGraphBehavior), 
            new PropertyMetadata(OnBindingChanged));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnBindingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var connectionWrapper = (ConnectionWrapper)d;
            connectionWrapper.OnBindingChanged(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnBindingChanged(DependencyPropertyChangedEventArgs e)
        {
            // nothing
        }

        /// <summary>
        /// 
        /// </summary>
        public Binding Binding { get { return (Binding)GetValue(BindingProperty); } set { SetValue(BindingProperty, value); } }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SingleConnectionWrapper : ConnectionWrapper { }    
}
