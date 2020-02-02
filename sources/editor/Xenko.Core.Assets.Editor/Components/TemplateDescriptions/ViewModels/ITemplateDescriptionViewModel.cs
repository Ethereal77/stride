// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

using Xenko.Core.Assets.Templates;

namespace Xenko.Core.Assets.Editor.Components.TemplateDescriptions.ViewModels
{
    public interface ITemplateDescriptionViewModel
    {
        string Name { get; }

        string Description { get; }

        string FullDescription { get; }

        string Group { get; }

        Guid Id { get; }

        string DefaultOutputName { get; }

        BitmapImage Icon { get; }

        IEnumerable<BitmapImage> Screenshots { get; }

        TemplateDescription GetTemplate();
    }
}
