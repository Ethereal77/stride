// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core
{
    /// <summary>
    ///   Represents the base class for framework components.
    /// </summary>
    [DataContract]
    public abstract class ComponentBase : DisposeBase, IComponent, ICollectorHolder
    {
        private string name;
        private ObjectCollector collector;

        /// <summary>
        ///   Initializes a new instance of the <see cref="ComponentBase"/> class.
        /// </summary>
        protected ComponentBase() : this(name: null) { }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ComponentBase"/> class.
        /// </summary>
        /// <param name="name">The name attached to this component. Specify <c>null</c> to use the name of the type automatically.</param>
        protected ComponentBase(string name)
        {
            collector = new ObjectCollector();
            Tags = new PropertyContainer(this);
            Name = name ?? GetType().Name;
        }

        /// <summary>
        ///   Gets the attached properties to this component.
        /// </summary>
        // Do not try to recreate object (preserve Tags.Owner)
        [DataMemberIgnore]
        public PropertyContainer Tags;

        /// <summary>
        ///   Gets or sets the name of this component.
        /// </summary>
        /// <value>The name.</value>
        // By default don't store it, unless derived class are overriding this member
        [DataMemberIgnore]
        public virtual string Name
        {
            get => name;

            set
            {
                if (value == name)
                    return;

                name = value;
                OnNameChanged();
            }
        }

        /// <summary>
        ///   Disposes the resources associated with this object.
        /// </summary>
        protected override void Destroy()
        {
            collector.Dispose();
        }

        ObjectCollector ICollectorHolder.Collector
        {
            get
            {
                collector.EnsureValid();
                return collector;
            }
        }

        /// <summary>
        ///   Method called when the <see cref="Name"/> property has changed.
        /// </summary>
        protected virtual void OnNameChanged() { }

        public override string ToString() => $"{GetType().Name}: {name}";
    }
}
