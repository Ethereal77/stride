// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;

using Stride.Core.Diagnostics;
using Stride.Core.Storage;
using Stride.Core.Serialization.Contents;

namespace Stride.Core.BuildEngine
{
    [DataContract, Serializable]
    [ContentSerializer(typeof(DataContentSerializer<CommandResultEntry>))]
    public class CommandResultEntry
    {
        public Dictionary<ObjectUrl, ObjectId> InputDependencyVersions;

        /// <summary>
        ///   Output object ids as saved in the object database.
        /// </summary>
        public Dictionary<ObjectUrl, ObjectId> OutputObjects;

        /// <summary>
        ///   Log messages corresponding to the execution of the command.
        /// </summary>
        public List<SerializableLogMessage> LogMessages;

        /// <summary>
        ///   Tags added for a given URL.
        /// </summary>
        public List<KeyValuePair<ObjectUrl, string>> TagSymbols;

        public CommandResultEntry()
        {
            InputDependencyVersions = new Dictionary<ObjectUrl, ObjectId>();
            OutputObjects = new Dictionary<ObjectUrl, ObjectId>();
            LogMessages = new List<SerializableLogMessage>();
            TagSymbols = new List<KeyValuePair<ObjectUrl, string>>();
        }
    }
}
