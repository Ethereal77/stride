// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.ComponentModel;

using Xenko.Core.Assets;
using Xenko.Core;
using Xenko.Core.Annotations;

namespace Xenko.Assets.Scripts
{
    [DataContract]
    public class Link : IIdentifiable, IAssetPartDesign<Link>
    {
        public Link()
        {
            Id = Guid.NewGuid();
        }

        public Link(Slot source, Slot target)
             : this()
        {
            Source = source;
            Target = target;
        }

        public Link(ExecutionBlock source, Slot target)
             : this()
        {
            Source = source.OutputExecution;
            Target = target;
        }

        public Link(Slot source, ExecutionBlock target)
     : this()
        {
            Source = source;
            Target = target.InputExecution;
        }

        public Link(ExecutionBlock source, ExecutionBlock target)
             : this()
        {
            Source = source.OutputExecution;
            Target = target.InputExecution;
        }

        /// <summary>
        /// The function that contains this link.
        /// </summary>
        [DataMemberIgnore]
        public Method Method { get; internal set; }


        [DataMember(-100), Display(Browsable = false)]
        [NonOverridable]
        public Guid Id { get; set; }

        /// <inheritdoc/>
        [DataMember(-90), Display(Browsable = false)]
        [DefaultValue(null)]
        public BasePart Base { get; set; }

        public Slot Source { get; set; }

        public Slot Target { get; set; }

        /// <inheritdoc/>
        IIdentifiable IAssetPartDesign.Part => this;

        /// <inheritdoc/>
        Link IAssetPartDesign<Link>.Part => this;
    }
}
