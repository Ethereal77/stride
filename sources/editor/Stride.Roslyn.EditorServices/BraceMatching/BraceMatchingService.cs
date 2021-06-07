// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;

namespace Stride.Roslyn.EditorServices.BraceMatching
{
    [Export(typeof(IBraceMatchingService))]
    internal class BraceMatchingService : IBraceMatchingService
    {
        private readonly ImmutableArray<Lazy<IBraceMatcher>> _braceMatchers;

        [ImportingConstructor]
        public BraceMatchingService(
            [ImportMany] IEnumerable<Lazy<IBraceMatcher>> braceMatchers)
        {
            _braceMatchers = braceMatchers.ToImmutableArray();
        }

        public async Task<BraceMatchingResult?> GetMatchingBracesAsync(Document document, int position, CancellationToken cancellationToken)
        {
            var text = await document.GetTextAsync(cancellationToken).ConfigureAwait(false);
            if (position < 0 || position > text.Length)
            {
                throw new ArgumentException(nameof(position));
            }

            foreach (var matcher in _braceMatchers)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var braces = await matcher.Value.FindBracesAsync(document, position, cancellationToken).ConfigureAwait(false);
                if (braces.HasValue)
                {
                    return braces;
                }
            }

            return null;
        }
    }
}
