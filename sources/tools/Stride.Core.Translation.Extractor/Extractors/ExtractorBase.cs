// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Stride.Core.Annotations;
using Stride.Core.IO;

namespace Stride.Core.Translation.Extractor
{
    internal abstract class ExtractorBase
    {
        private readonly string[] supportedExtensions;

        protected ExtractorBase([NotNull] ICollection<UFile> inputFiles, params string[] supportedExtensions)
        {
            this.supportedExtensions = supportedExtensions;
            InputFiles = inputFiles ?? throw new ArgumentNullException(nameof(inputFiles));
        }

        protected ICollection<UFile> InputFiles { get; }

        [NotNull]
        public IReadOnlyList<Message> ExtractMessages()
        {
            return InputFiles.Where(f => supportedExtensions.Contains(f.GetFileExtension())).SelectMany(ExtractMessagesFromFile).ToList();
        }

        [ItemNotNull]
        protected abstract IEnumerable<Message> ExtractMessagesFromFile([NotNull] UFile file);
    }
}
