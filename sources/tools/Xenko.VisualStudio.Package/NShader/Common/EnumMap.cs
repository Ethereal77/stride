// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2009 NShader - Alexandre Mutel, Microsoft Corporation
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace NShader
{

    public class EnumMap<T> : Dictionary<string, T>
    {
        public void Load(string resource)
        {
            Stream file = typeof(T).Assembly.GetManifestResourceStream(typeof(Xenko.VisualStudio.Resources).Namespace + ".NShader.Common." + resource);
            TextReader textReader = new StreamReader(file);
            string line;
            while ((line = textReader.ReadLine()) != null )
            {
                int indexEqu = line.IndexOf('=');
                if ( indexEqu > 0 )
                {
                    string enumName = line.Substring(0, indexEqu);
                    string value = line.Substring(indexEqu + 1, line.Length - indexEqu-1).Trim();
                    string[] values = Regex.Split(value, @"[\t ]+");
                    T enumValue = (T)Enum.Parse(typeof(T), enumName);
                    foreach (string token in values)
                    {
                        if (!ContainsKey(token))
                        {
                            Add(token, enumValue);
                        } else
                        {
                            Trace.WriteLine(string.Format("Warning: token {0} for enum {1} already added for {2}", token, enumValue, this[token]));
                        }
                    }
                }

            }
        }
    }
}
