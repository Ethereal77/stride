// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Presentation.Quantum.Presenters;
using Stride.Core.Presentation.Quantum.ViewModels;
using Stride.Core.Quantum;

namespace Stride.Core.Presentation.Quantum.Tests.Helpers
{
    public class TestInstanceContext
    {
        private readonly TestContainerContext context;

        public TestInstanceContext(TestContainerContext context, IObjectNode rootNode)
        {
            this.context = context;
            RootNode = rootNode;
            PropertyProvider = new Types.TestPropertyProvider(rootNode);
        }

        public IPropertyProviderViewModel PropertyProvider { get; }

        public IObjectNode RootNode { get; }

        public INodePresenterFactory Factory => context.GraphViewModelService.NodePresenterFactory;

        public GraphViewModel CreateViewModel()
        {
            return GraphViewModel.Create(context.ServiceProvider, new[] { PropertyProvider });
        }
    }
}
