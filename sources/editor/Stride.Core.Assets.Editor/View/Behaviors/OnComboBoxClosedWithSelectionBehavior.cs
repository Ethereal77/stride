// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Windows.Controls;

using Xenko.Core.Presentation.Behaviors;

namespace Xenko.Core.Assets.Editor.View.Behaviors
{
    public class OnComboBoxClosedWithSelectionBehavior : OnEventCommandBehavior
    {
        public OnComboBoxClosedWithSelectionBehavior()
        {
            EventName = "DropDownClosed";
        }

        protected override void OnAttached()
        {
            if (!(AssociatedObject is ComboBox))
                throw new InvalidOperationException("The OnComboBoxClosedWithSelectionBehavior must be attached to a ComboBox.");

            base.OnAttached();
        }

        protected override void OnEvent()
        {
            var comboBox = (ComboBox)AssociatedObject;
            if (comboBox.SelectedItem != null)
                base.OnEvent();
        }
    }
}
