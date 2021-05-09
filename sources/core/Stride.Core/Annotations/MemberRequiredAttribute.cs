// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.Annotations
{
    /// <summary>
    ///   Marks a field or property as requiring to hava a value (i.e. not <c>null</c>) and signals the Asset Compiler
    ///   to enforce this rule when compiling Assets.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class MemberRequiredAttribute : Attribute
    {
        /// <summary>
        ///   Gets or sets the reporting level (warning / error) of the notification when the annotated
        ///   field or property has no value.
        /// </summary>
        public MemberRequiredReportType ReportAs { get; set; } = MemberRequiredReportType.Warning;
    }


    /// <summary>
    ///   Defines the reporting levels for a missing value in a field or property annotated with a
    ///   <see cref="MemberRequiredAttribute"/> attribute.
    /// </summary>
    public enum MemberRequiredReportType
    {
        /// <summary>
        ///   Reports the missing required member as a warning.
        /// </summary>
        Warning,

        /// <summary>
        ///   Reports the missing required member as an error.
        /// </summary>
        Error
    }
}
