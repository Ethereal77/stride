// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Core.MicroThreading
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class XenkoScriptAttribute : Attribute
    {
        public XenkoScriptAttribute(ScriptFlags flags = ScriptFlags.None)
        {
            this.Flags = flags;
        }

        public ScriptFlags Flags { get; set; }
    }
}
