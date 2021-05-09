// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Xunit;

using Stride.Rendering;

namespace Stride.Shaders.Tests
{
    /// <summary>
    /// Helper methods for TestMixinGenerator
    /// </summary>
    public partial class TestMixinGenerator
    {
        /// <summary>
        /// Generates the mixin.
        /// </summary>
        /// <param name="mixinName">Name of the mixin.</param>
        /// <param name="properties">The properties that the mixin will use.</param>
        /// <returns>ShaderMixinSource.</returns>
        private static ShaderMixinSource GenerateMixin(string mixinName, ParameterCollection properties)
        {
            var mixin = ShaderMixinManager.Generate(mixinName, properties);

            // Verify that output used properties are a subset of input properties
            //Assert.That(usedProperties.IsSubsetOf(properties), Is.True);

            //foreach(var usedProps in allUsedProperties)
            //    Assert.That(usedProps.IsSubsetOf(properties), Is.True);

            return mixin;
        }
    }
}
