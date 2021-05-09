// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2020 Tebjan Halm
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Core.Annotations;
using Stride.Engine.Design;
using Stride.Engine.Processors;

namespace Stride.Engine
{
    /// <summary>
    /// Represents a single instance of a model instancing component.
    /// </summary>
    /// <seealso cref="Stride.Engine.ActivableEntityComponent" />
    [DataContract("InstanceComponent")]
    [Display("Instance", Expand = ExpandRule.Once)]
    [ComponentCategory("Model")]
    [DefaultEntityComponentProcessor(typeof(InstanceProcessor))]
    public sealed class InstanceComponent : ActivableEntityComponent
    {
        private InstancingComponent master;
        private InstancingEntityTransform connectedInstancing;

        /// <summary>
        /// Gets or sets the referenced <see cref="InstancingComponent"/> to instance.
        /// </summary>
        /// <value>The referenced <see cref="InstancingComponent"/> to instance.</value>
        /// <userdoc>The "Master" <see cref="InstancingComponent"/> to instance. If not set, it tries to find one in the parent entities</userdoc>
        [DataMember(10)]
        [NotNull]
        [Display("Instancing", Expand = ExpandRule.Always)]
        public InstancingComponent Master
        {
            get => master;
            set
            {
                if (value != master)
                {
                    DisconnectInstancing();

                    // Remove previous event handler
                    if (master != null)
                        master.InstancingChanged -= Master_InstancingChanged;

                    master = value;

                    if (master != null)
                    {
                        master.InstancingChanged += Master_InstancingChanged;
                        ConnectInstancing();
                    }
                }
            }
        }

        private void Master_InstancingChanged(object sender, IInstancing e)
        {
            DisconnectInstancing();
            ConnectInstancing();
        }

        internal void ConnectInstancing()
        {
            if (connectedInstancing == null && master != null && master.Type is InstancingEntityTransform instancing)
            {
                instancing.AddInstance(this);
                connectedInstancing = instancing;
            }
        }

        internal void DisconnectInstancing()
        {
            if (connectedInstancing != null)
            {
                connectedInstancing.RemoveInstance(this);
                connectedInstancing = null;
            }
        }
    }
}
