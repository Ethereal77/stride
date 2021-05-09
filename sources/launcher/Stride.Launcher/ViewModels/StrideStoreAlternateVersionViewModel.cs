// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Stride.Core;
using Stride.Core.Annotations;
using Stride.Core.Packages;
using Stride.Core.Presentation.Commands;
using Stride.Core.Presentation.ViewModel;

namespace Stride.LauncherApp.ViewModels
{
    internal sealed class StrideStoreAlternateVersionViewModel : DispatcherViewModel
    {
        private StrideStoreVersionViewModel strideVersion;
        internal NugetServerPackage ServerPackage;
        internal NugetLocalPackage LocalPackage;

        public StrideStoreAlternateVersionViewModel([NotNull] StrideStoreVersionViewModel strideVersion)
            : base(strideVersion.ServiceProvider)
        {
            this.strideVersion = strideVersion;

            SetAsActiveCommand = new AnonymousCommand(ServiceProvider, () =>
            {
                strideVersion.UpdateLocalPackage(LocalPackage, null);
                if (LocalPackage == null)
                {
                    // If it's a non installed version, offer same version for serverPackage so that it offers to install this specific version
                    strideVersion.UpdateServerPackage(ServerPackage, null);
                }
                else
                {
                    // Otherwise, offer latest version for update
                    strideVersion.UpdateServerPackage(strideVersion.LatestServerPackage, null);
                }

                strideVersion.Launcher.ActiveVersion = strideVersion;
            });
        }

        /// <summary>
        /// Gets the command that will set the associated version as active.
        /// </summary>
        public CommandBase SetAsActiveCommand { get; }

        public string FullName
        {
            get
            {
                if (LocalPackage != null)
                    return $"{LocalPackage.Id} {LocalPackage.Version} (installed)";
                return $"{ServerPackage.Id} {ServerPackage.Version}";
            }
        }

        public PackageVersion Version => LocalPackage?.Version ?? ServerPackage.Version;

        internal void UpdateLocalPackage(NugetLocalPackage package)
        {
            OnPropertyChanging(nameof(FullName), nameof(Version));
            LocalPackage = package;
            OnPropertyChanged(nameof(FullName), nameof(Version));
        }

        internal void UpdateServerPackage(NugetServerPackage package)
        {
            OnPropertyChanging(nameof(FullName), nameof(Version));
            ServerPackage = package;
            OnPropertyChanged(nameof(FullName), nameof(Version));
        }
    }
}
