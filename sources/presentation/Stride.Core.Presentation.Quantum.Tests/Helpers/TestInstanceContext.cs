// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Presentation.Quantum.Presenters;
using Xenko.Core.Presentation.Quantum.ViewModels;
using Xenko.Core.Quantum;

namespace Xenko.Core.Presentation.Quantum.Tests.Helpers
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
