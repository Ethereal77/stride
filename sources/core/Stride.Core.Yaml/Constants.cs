// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Yaml.Tokens;

namespace Stride.Core.Yaml
{
    /// <summary>
    /// Defines constants thar relate to the YAML specification.
    /// </summary>
    internal static class Constants
    {
        public static readonly TagDirective[] DefaultTagDirectives = new[]
        {
            new TagDirective("!", "!"),
            new TagDirective("!!", "tag:yaml.org,2002:"),
        };

        public const int MajorVersion = 1;
        public const int MinorVersion = 1;

        public const char HandleCharacter = '!';
        public const string DefaultHandle = "!";
    }
}