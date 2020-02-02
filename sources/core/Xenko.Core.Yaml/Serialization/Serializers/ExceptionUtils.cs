// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Reflection;

namespace Xenko.Core.Yaml.Serialization.Serializers
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
