// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.ComponentModel;

using Stride.Core;

namespace Stride.Rendering.Materials
{
    /// <summary>
    /// A material feature
    /// </summary>
    [DataContract(Inherited = true)]
    public abstract class MaterialFeature : IMaterialFeature
    {
        [DataMember(-20)]
        [DefaultValue(true)]
        public bool Enabled { get; set; } = true;

        protected MaterialFeature()
        {
        }

        public void Visit(MaterialGeneratorContext context)
        {
            if (!Enabled)
                return;

            switch (context.Step)
            {
                case MaterialGeneratorStep.PassesEvaluation:
                    MultipassGeneration(context);
                    break;
                case MaterialGeneratorStep.GenerateShader:
                    GenerateShader(context);
                    break;
            }
        }

        /// <summary>
        /// Called during prepass, used to enumerate extra passes.
        /// </summary>
        /// <param name="context">The context.</param>
        public virtual void MultipassGeneration(MaterialGeneratorContext context)
        {
        }

        /// <summary>
        /// Generates the shader for the feature.
        /// </summary>
        /// <param name="context">The context.</param>
        public abstract void GenerateShader(MaterialGeneratorContext context);
    }
}
