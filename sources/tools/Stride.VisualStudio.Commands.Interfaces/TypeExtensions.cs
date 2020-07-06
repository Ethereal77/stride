// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Stride.VisualStudio.Commands
{
    static class TypeExtensions
    {
        public static Type ToType(this string configName)
        {
            try
            {
                var result = Type.GetType(configName);
                return result;
                //var parts = (from n in configName.Split(',') select n.Trim()).ToArray();
                //var assembly = Assembly.Load(new AssemblyName(parts[1]));
                //var type = assembly.GetType(parts[0]);
                //return type;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return null;
        }
    }
}
