// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

// Deliberate copy to make the type private
namespace Stride.Core.Annotations
{
    /// <summary>
    ///   Indicates that the value of the marked element could never be <c>null</c>.
    /// </summary>
    /// <example>
    /// <code>
    /// [NotNull]
    /// object Foo()
    /// {
    ///   return null; // Warning: Possible 'null' assignment
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(
         AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property |
         AttributeTargets.Delegate | AttributeTargets.Field | AttributeTargets.Event |
         AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.GenericParameter)]
    internal sealed class NotNullAttribute : Attribute { }
}
