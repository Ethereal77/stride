// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// See the LICENSE.md file in the project root for full license information.

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Stride.Core.Yaml.Tests
{
    public class YamlTest
    {
        protected static TextReader YamlFile(string name)
        {
            var fromType = typeof(YamlTest);
            var assembly = Assembly.GetAssembly(fromType);
            var stream = assembly.GetManifestResourceStream(name) ??
                         assembly.GetManifestResourceStream(fromType.Namespace + ".files." + name);
            return new StreamReader(stream);
        }

        protected static TextReader YamlText(string yaml)
        {
            var lines = yaml
                .Split('\n')
                .Select(l => l.TrimEnd('\r', '\n'))
                .SkipWhile(l => l.Trim(' ', '\t').Length == 0)
                .ToList();

            while (lines.Count > 0 && lines[lines.Count - 1].Trim(' ', '\t').Length == 0)
            {
                lines.RemoveAt(lines.Count - 1);
            }

            if (lines.Count > 0)
            {
                var indent = Regex.Match(lines[0], @"^(\t+)");
                if (!indent.Success)
                {
                    throw new ArgumentException("Invalid indentation");
                }

                lines = lines
                    .Select(l => l.Substring(indent.Groups[1].Length))
                    .ToList();
            }

            return new StringReader(string.Join("\n", lines.ToArray()));
        }
    }
}
