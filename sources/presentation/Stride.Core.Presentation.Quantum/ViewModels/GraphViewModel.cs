// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;

using Stride.Core.Annotations;
using Stride.Core.Diagnostics;
using Stride.Core.Extensions;
using Stride.Core.Quantum;
using Stride.Core.Presentation.Services;
using Stride.Core.Presentation.ViewModel;
using Stride.Core.Presentation.Quantum.Presenters;

namespace Stride.Core.Presentation.Quantum.ViewModels
{
    /// <summary>
    ///   Represents a view model for one or multiple trees of <see cref="INodePresenter"/> instances.
    /// </summary>
    public class GraphViewModel : DispatcherViewModel
    {
        public const string DefaultLoggerName = "Quantum";
        public const string HasChildPrefix = "HasChild_";
        public const string HasCommandPrefix = "HasCommand_";
        public const string HasAssociatedDataPrefix = "HasAssociatedData_";

        private NodeViewModel rootNode;


        /// <summary>
        ///  Initializes a new instance of the <see cref="GraphViewModel"/> class.
        /// </summary>
        /// <param name="serviceProvider">
        ///   A service provider that can provide a <see cref="IDispatcherService"/> and a
        ///   <see cref="Quantum.GraphViewModelService"/> to use for this view model.
        /// </param>
        /// <param name="type">The type for which to generate the graph view model.</param>
        /// <param name="rootPresenters">The root <see cref="INodePresenter"/> instances.</param>
        private GraphViewModel([NotNull] IViewModelServiceProvider serviceProvider, [NotNull] Type type, [NotNull] IEnumerable<INodePresenter> rootPresenters)
            : base(serviceProvider)
        {
            if (serviceProvider is null)
                throw new ArgumentNullException(nameof(serviceProvider));
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            if (rootPresenters is null)
                throw new ArgumentNullException(nameof(rootPresenters));

            GraphViewModelService = serviceProvider.TryGet<GraphViewModelService>();
            if (GraphViewModelService is null)
                throw new InvalidOperationException($"{nameof(GraphViewModel)} requires a {nameof(GraphViewModelService)} in the service provider.");

            Logger = GlobalLogger.GetLogger(DefaultLoggerName);

            var viewModelFactory = GraphViewModelService.NodeViewModelFactory;
            viewModelFactory.CreateGraph(this, type, rootPresenters);
        }

        /// <inheritdoc/>
        public override void Destroy()
        {
            RootNode.Children.SelectDeep(x => x.Children)
                             .ForEach(x => x.Destroy());

            RootNode.Destroy();
        }

        [CanBeNull]
        public static GraphViewModel Create([NotNull] IViewModelServiceProvider serviceProvider, [NotNull] IReadOnlyCollection<IPropertyProviderViewModel> propertyProviders)
        {
            if (serviceProvider is null)
                throw new ArgumentNullException(nameof(serviceProvider));
            if (propertyProviders is null)
                throw new ArgumentNullException(nameof(propertyProviders));

            var rootNodes = new List<INodePresenter>();
            Type type = null;
            var factory = serviceProvider.Get<GraphViewModelService>().NodePresenterFactory;
            foreach (var propertyProvider in propertyProviders)
            {
                if (!propertyProvider.CanProvidePropertiesViewModel)
                    return null;

                var rootNode = propertyProvider.GetRootNode();
                if (rootNode is null)
                    return null;

                if (type is null)
                    type = rootNode.Type;
                else if (type != rootNode.Type)
                    return null;

                var node = factory.CreateNodeHierarchy(rootNode, new GraphNodePath(rootNode), propertyProvider);
                rootNodes.Add(node);
            }

            if (propertyProviders.Count == 0 || type is null)
                throw new ArgumentException($@"{nameof(propertyProviders)} cannot be empty.", nameof(propertyProviders));

            return new GraphViewModel(serviceProvider, type, rootNodes);
        }

        /// <summary>
        ///   Gets or sets the root node of this <see cref="GraphViewModel"/>.
        /// </summary>
        public NodeViewModel RootNode
        {
            get => rootNode;
            set => SetValue(ref rootNode, value);
        }

        /// <summary>
        ///   Gets the <see cref="Quantum.GraphViewModelService"/> associated to this view model.
        /// </summary>
        public GraphViewModelService GraphViewModelService { get; }

        /// <summary>
        ///   Gets the <see cref="Stride.Core.Diagnostics.Logger"/> associated to this view model.
        /// </summary>
        public Logger Logger { get; private set; }

        /// <summary>
        ///   Event raised when the value of an <see cref="NodeViewModel"/> contained into this view model has changed.
        /// </summary>
        public event EventHandler<NodeViewModelValueChangedArgs> NodeValueChanged;

        internal void NotifyNodeChanged(NodeViewModel node)
        {
            NodeValueChanged?.Invoke(this, new NodeViewModelValueChangedArgs(this, node));
        }
    }
}
