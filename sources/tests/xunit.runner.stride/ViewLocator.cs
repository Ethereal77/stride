// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using System;

using Avalonia.Controls;
using Avalonia.Controls.Templates;

using xunit.runner.stride.ViewModels;

namespace xunit.runner.stride
{
    public class ViewLocator : IDataTemplate
    {
        public bool SupportsRecycling => false;

        public IControl Build(object data)
        {
            var name = data.GetType().FullName.Replace("ViewModel", "View");
            var type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type);
            }
            else
            {
                return new TextBlock { Text = "Not Found: " + name };
            }
        }

        public bool Match(object data)
        {
            return data is ViewModelBase;
        }
    }
}
