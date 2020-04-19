// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Diagnostics;

namespace Xenko.Core.Yaml.Tests
{
    public class Dump
    {
        [Conditional("TEST_DUMP")]
        public static void Write(object value)
        {
            Debug.Write(value);
        }

        [Conditional("TEST_DUMP")]
        public static void Write(string format, params object[] args)
        {
            Debug.Write(string.Format(format, args));
        }

        [Conditional("TEST_DUMP")]
        public static void WriteLine()
        {
            Debug.WriteLine(string.Empty);
        }

        [Conditional("TEST_DUMP")]
        public static void WriteLine(string value)
        {
            WriteLine((object) value);
        }

        [Conditional("TEST_DUMP")]
        public static void WriteLine(object value)
        {
            WriteLine("{0}", value);
        }

        [Conditional("TEST_DUMP")]
        public static void WriteLine(string format, params object[] args)
        {
            Debug.WriteLine(format, args);
        }
    }
}