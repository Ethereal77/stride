// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Stride.Core.Assets.Templates;
using Stride.Core.IO;

namespace Stride.Core.Assets.Editor.ViewModel
{
    public class NewSessionParameters
    {
        public TemplateDescription TemplateDescription;
        public string OutputName;
        public UDirectory OutputDirectory;
        public string SolutionName;
        public UDirectory SolutionLocation;
    }
}
