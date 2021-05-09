// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Stride.Core.Annotations;

namespace Stride.Core.Presentation.Quantum.Presenters
{
    public abstract class NodePresenterCommandBase : INodePresenterCommand
    {
        public abstract string Name { get; }

        public virtual CombineMode CombineMode => CombineMode.CombineOnlyForAll;

        public abstract bool CanAttach(INodePresenter nodePresenter);

        public virtual bool CanExecute(IReadOnlyCollection<INodePresenter> nodePresenters, object parameter)
        {
            return true;
        }

        [NotNull]
        public virtual Task<object> PreExecute(IReadOnlyCollection<INodePresenter> nodePresenters, object parameter)
        {
            return Task.FromResult<object>(null);
        }

        public abstract Task Execute(INodePresenter nodePresenter, object parameter, object preExecuteResult);

        [NotNull]
        public virtual Task PostExecute(IReadOnlyCollection<INodePresenter> nodePresenters, object parameter)
        {
            return Task.CompletedTask;
        }
    }
}
