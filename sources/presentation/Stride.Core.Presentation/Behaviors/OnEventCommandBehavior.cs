// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Windows;
using System.Windows.Input;

namespace Stride.Core.Presentation.Behaviors
{
    /// <summary>
    /// An implementation of the <see cref="OnEventBehavior"/> class that allows to invoke a command when a specific event is raised.
    /// </summary>
    public class OnEventCommandBehavior : OnEventBehavior
    {
        /// <summary>
        /// Identifies the <see cref="Command"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(OnEventCommandBehavior));

        /// <summary>
        /// Identifies the <see cref="CommandParameter"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(OnEventCommandBehavior));

        /// <summary>
        /// Gets or sets the command to invoke when the event is raised.
        /// </summary>
        public ICommand Command { get { return (ICommand)GetValue(CommandProperty); } set { SetValue(CommandProperty, value); } }

        /// <summary>
        /// Gets or sets the parameter of the command to invoke when the event is raised.
        /// </summary>
        public object CommandParameter { get { return GetValue(CommandParameterProperty); } set { SetValue(CommandParameterProperty, value); } }

        /// <inheritdoc/>
        protected override void OnEvent()
        {
            var cmd = Command;

            if (cmd != null && cmd.CanExecute(CommandParameter))
                cmd.Execute(CommandParameter);
        }
    }
}
