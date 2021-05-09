// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Threading.Tasks;

namespace Stride.Core.Packages
{
    /// <summary>
    /// Null implementation of <see cref="IPackagesLogger"/>.
    /// </summary>
    public class NullPackagesLogger : IPackagesLogger
    {
        public static IPackagesLogger Instance { get; } = new NullPackagesLogger();

        public void Log(MessageLevel level, string message)
        {
        }

        public Task LogAsync(MessageLevel level, string message)
        {
            return Task.CompletedTask;
        }
    }
}
