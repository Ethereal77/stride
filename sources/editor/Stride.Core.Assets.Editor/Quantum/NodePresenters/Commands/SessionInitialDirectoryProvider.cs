// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Linq;

using Stride.Core.Assets.Editor.ViewModel;
using Stride.Core.IO;

namespace Stride.Core.Assets.Editor.Quantum.NodePresenters.Commands
{
    public class SessionInitialDirectoryProvider : IInitialDirectoryProvider
    {
        private readonly SessionViewModel session;

        public SessionInitialDirectoryProvider(SessionViewModel session)
        {
            this.session = session;
        }

        public UDirectory GetInitialDirectory(UDirectory currentPath)
        {
            if (session != null && currentPath != null)
            {
                // Take the solution directory by default.
                var sessionPath = session.SolutionPath;
                if (sessionPath == null)
                {
                    // If there is no solution directory, try to use the directory of the first local package.
                    var firstPackage = session.LocalPackages.FirstOrDefault();
                    if (firstPackage != null)
                        sessionPath = firstPackage.PackagePath;
                }

                if (sessionPath != null)
                {
                    var defaultPath = UPath.Combine(sessionPath.GetFullDirectory(), currentPath);
                    return defaultPath.GetFullDirectory();
                }
            }
            return currentPath;
        }
    }
}
