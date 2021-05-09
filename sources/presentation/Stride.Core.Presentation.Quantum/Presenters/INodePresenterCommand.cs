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
    public interface INodePresenterCommand
    {
        [NotNull]
        string Name { get; }

        CombineMode CombineMode { get; }

        bool CanAttach([NotNull] INodePresenter nodePresenter);

        bool CanExecute([NotNull] IReadOnlyCollection<INodePresenter> nodePresenters, object parameter);

        Task<object> PreExecute([NotNull] IReadOnlyCollection<INodePresenter> nodePresenters, object parameter);

        Task Execute([NotNull] INodePresenter nodePresenter, object parameter, object preExecuteResult);

        Task PostExecute([NotNull] IReadOnlyCollection<INodePresenter> nodePresenters, object parameter);
    }
}
