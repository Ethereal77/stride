// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Reflection;

using Stride.Core.Presentation.ViewModel;

namespace Stride.ConfigEditor.ViewModels
{
    public class PropertyViewModel : ViewModelBase
    {
        public SectionViewModel Parent { get; private set; }
        public PropertyInfo Property { get; private set; }
        public ConfigurationPropertyAttribute Attribute { get; private set; }

        public PropertyViewModel(SectionViewModel parent, PropertyInfo property, ConfigurationPropertyAttribute attribute)
        {
            if (parent is null)
                throw new ArgumentNullException(nameof(parent));
            if (property is null)
                throw new ArgumentNullException(nameof(property));
            if (attribute is null)
                throw new ArgumentNullException(nameof(attribute));

            Parent = parent;

            Property = property;
            Attribute = attribute;

            DefaultValue = Attribute.DefaultValue;
            Value = DefaultValue;
        }

        private bool isUsed;
        public bool IsUsed
        {
            get => isUsed;
            set => SetValue(ref isUsed, value, nameof(IsUsed));
        }

        public string PropertyName => Property.Name;
        public string PropertyTypeName => Property.PropertyType.FullName;

        public object DefaultValue { get; private set; }

        private object value;
        public object Value
        {
            get => value;
            set => SetValue(ref this.value, value, nameof(Value));
        }

        public bool IsRequired => Attribute.IsRequired;
    }
}
