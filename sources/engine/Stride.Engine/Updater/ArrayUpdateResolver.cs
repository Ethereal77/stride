// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Globalization;

namespace Stride.Updater
{
    /// <summary>
    /// Resolver for <see cref="T[]"/> in property path.
    /// </summary>
    /// <typeparam name="T">The type of array items.</typeparam>
    public class ArrayUpdateResolver<T> : UpdateMemberResolver
    {
        public override Type SupportedType => typeof(T[]);

        public override UpdatableMember ResolveIndexer(string indexerName)
        {
            // Transform index into integer
            int indexerValue;
            if (!int.TryParse(indexerName, NumberStyles.Any, CultureInfo.InvariantCulture, out indexerValue))
                throw new InvalidOperationException(string.Format("Property path parse error: could not parse indexer value '{0}'", indexerName));

            var updatableField = new UpdatableArrayAccessor<T>(indexerValue);

            return updatableField;
        }
    }
}
