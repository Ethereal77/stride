// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Stride.Core.Annotations;

namespace Stride.Core.Extensions
{
    /// <summary>
    ///   Defines extensions for operating with <see cref="Task"/>s.
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        ///   Does nothing with a <see cref="Task"/>, allowing it to complete or fail.
        /// </summary>
        /// <param name="task">The <see cref="Task"/> to forget.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Forget([NotNull] this Task task)
        {
            if (task is null)
                throw new ArgumentNullException();
        }
    }
}
