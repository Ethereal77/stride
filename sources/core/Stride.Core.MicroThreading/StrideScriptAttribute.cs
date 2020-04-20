// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Stride.Core.MicroThreading
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class StrideScriptAttribute : Attribute
    {
        public StrideScriptAttribute(ScriptFlags flags = ScriptFlags.None)
        {
            this.Flags = flags;
        }

        public ScriptFlags Flags { get; set; }
    }
}
