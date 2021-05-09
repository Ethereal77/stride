// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Reflection;

namespace Stride.Core.Yaml.Serialization.Serializers
{
    internal static class ExceptionUtils
    {
        /// <summary>
        /// Unwraps some exception such as <see cref="TargetInvocationException"/>.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static Exception Unwrap(this Exception exception)
        {
            var targetInvocationException = exception as TargetInvocationException;
            if (targetInvocationException != null)
                return targetInvocationException.InnerException;

            return exception;
        }
    }
}
