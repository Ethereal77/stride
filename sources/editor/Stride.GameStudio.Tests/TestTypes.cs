// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Linq;
using System.Reflection;

using Xunit;

using Stride.Core.Assets;
using Stride.Core;
using Stride.Core.Annotations;
using Stride.Core.Reflection;

namespace Stride.GameStudio.Tests
{
    public class TestTypes
    {
        /// <summary>
        /// This is not a test method but prints nullable items accepted for collections.
        /// TODO: we could ensure that only a few classes are allowed to allow this.
        /// </summary>
        [Fact]
        public void CollectNullableItemsInCollectionTypes()
        {
            foreach (var assembly in AssetRegistry.AssetAssemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    var hasDataCotnract = type.GetCustomAttribute<DataContractAttribute>() != null;

                    if (hasDataCotnract)
                    {
                        var typeDesc = TypeDescriptorFactory.Default.Find(type);

                        foreach (var member in typeDesc.Members)
                        {
                            var collectionDesc = member.TypeDescriptor as CollectionDescriptor;
                            if (collectionDesc != null && !collectionDesc.ElementType.IsValueType)
                            {
                                if (member.GetCustomAttributes<MemberCollectionAttribute>(true).FirstOrDefault()?.NotNullItems ?? false)
                                {
                                    Console.WriteLine($"Collection item non nullable type: {type}.{member.Name}");
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
