// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Framework.MicroThreading;

using Stride.Core.Presentation.Commands;
using Stride.Core.Presentation;

namespace Stride.DebugTools.ViewModels
{
    public class MicroThreadViewModel : DeprecatedViewModelBase
    {
        private readonly MicroThread microThread;

        public MicroThreadViewModel(MicroThread microThread)
        {
            if (microThread is null)
                throw new ArgumentNullException(nameof(microThread));

            if (microThread.Scheduler is null)
                throw new ArgumentException("Invalid Scheduler in MicroThread " + microThread.Id);

            this.microThread = microThread;

            // New MicroThread system doesn't have any PropertyChanged event yet.
            throw new NotImplementedException();
            //this.microThread.Scheduler.MicroThreadStateChanged += OnMicroThreadStateChanged;
        }

        private void OnMicroThreadStateChanged(object sender, SchedulerEventArgs e)
        {
            if (e.MicroThread == microThread)
            {
                OnPropertyChanged<MicroThreadViewModel>(n => n.State);
            }
        }

        public long Id => microThread.Id;

        public string Name => microThread.Name;

        public MicroThreadState State => microThread.State;

        public Exception Exception => microThread.Exception;
    }
}
