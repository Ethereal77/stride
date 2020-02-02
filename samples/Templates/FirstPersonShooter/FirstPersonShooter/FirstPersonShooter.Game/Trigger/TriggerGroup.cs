// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;

using Xenko.Core;

namespace FirstPersonShooter.Trigger
{
    [DataContract("TriggerGroup")]
    public class TriggerGroup
    {
        [DataMember(10)]
        [Display("Name")]
        public string Name { get; set; } = "NewTriggerGroup";

        [DataMember(20)]
        [Display("Events")]
        public List<TriggerEvent> TriggerEvents { get; } = new List<TriggerEvent>();

        public TriggerEvent Find(string name) => Find(x => x.Name.Equals(name));

        public List<TriggerEvent> FindAll(Predicate<TriggerEvent> match)
        {
            return TriggerEvents.FindAll(match);
        }

        public TriggerEvent Find(Predicate<TriggerEvent> match)
        {
            return TriggerEvents.Find(match);
        }
    }
}
