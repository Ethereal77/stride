// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2016 JetBrains http://www.jetbrains.com
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.Annotations
{
    /// <summary>
    /// Indicates that the value of the marked element could be <c>null</c> sometimes, so the check for <c>null</c>
    /// is necessary before its usage.
    /// </summary>
    /// <example>
    /// <code>
    /// [CanBeNull] object Test() => null;
    ///
    /// void UseTest() {
    ///   var p = Test();
    ///   var s = p.ToString(); // Warning: Possible 'System.NullReferenceException'
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(
         AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property |
         AttributeTargets.Delegate | AttributeTargets.Field | AttributeTargets.Event |
         AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.GenericParameter)]
    public sealed class CanBeNullAttribute : Attribute { }
}
