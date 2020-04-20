// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#if DEBUG

using System.Collections.Generic;

using Stride.Core.Assets;
using Stride.Core;
using Stride.Core.Mathematics;

namespace Stride.Assets.Presentation.Test
{
    [DataContract("UnitTestAsset")]
    [AssetDescription(FileExtension)]
    [Display("Unit Test Asset")]
    public sealed class UnitTestAsset : Asset
    {
        public const string FileExtension = ".xkunittest";

        //[DataMember(270)]
        //[Display("List of Class Lists", "Collections")]
        //public List<TestClass> MyCollection { get; set; } = new List<TestClass>();

        public Vector3 BoxedValue { get; set; } = new Vector3(3, 4, 5);

        public static UnitTestAsset CreateNew()
        {
            var testAsset = new UnitTestAsset
            {
                //MyCollection =
                //{
                //    new TestClass(),
                //    new TestClass()
                //}
            };
            //testAsset.BoxedValue = 5.3f;
            return testAsset;
        }
    }
}

#endif
