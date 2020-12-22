// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Stride.Games
{
    /// <summary>
    ///   Represents the collection of parameters used when launching an application.
    /// </summary>
    public class LaunchParameters : Dictionary<string, string>
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="LaunchParameters" /> class.
        /// </summary>
        public LaunchParameters()
        {
            var args = Environment.GetCommandLineArgs();

            if (args.Length > 1)
            {
                var trimChars = new[] { '/', '-' };

                for (int i = 1; i < args.Length; i++)
                {
                    var argument = args[i].TrimStart(trimChars);
                    string key;
                    var value = string.Empty;

                    int index = argument.IndexOf(':');
                    if (index != -1)
                    {
                        key = argument.Substring(0, index);
                        value = argument.Substring(index + 1);
                    }
                    else
                    {
                        key = argument;
                    }

                    if (!ContainsKey(key) && (key != string.Empty))
                    {
                        Add(key, value);
                    }
                }
            }
        }
    }
}
