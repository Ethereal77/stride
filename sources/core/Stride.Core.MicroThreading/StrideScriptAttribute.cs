// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

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
