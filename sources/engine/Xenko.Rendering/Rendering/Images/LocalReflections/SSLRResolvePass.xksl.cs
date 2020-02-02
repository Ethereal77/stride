// <auto-generated>
// Do not edit this file yourself!
//
// This code was generated by Xenko Shader Mixin Code Generator.
// To generate it yourself, please install Xenko.VisualStudio.Package .vsix
// and re-save the associated .xkfx.
// </auto-generated>

using System;

using Xenko.Core;
using Xenko.Rendering;
using Xenko.Graphics;
using Xenko.Shaders;
using Xenko.Core.Mathematics;
using Buffer = Xenko.Graphics.Buffer;

namespace Xenko.Rendering.Images
{
    public static partial class SSLRResolvePassKeys
    {
    }
    internal static partial class ShaderMixins
    {
        internal partial class SSLRResolvePassEffect  : IShaderMixinBuilder
        {
            public void Generate(ShaderMixinSource mixin, ShaderMixinContext context)
            {
                context.Mixin(mixin, "SSLRResolvePass", context.GetParam(SSLRKeys.ResolveSamples), context.GetParam(SSLRKeys.ReduceHighlights));
            }

            [ModuleInitializer]
            internal static void __Initialize__()

            {
                ShaderMixinManager.Register("SSLRResolvePassEffect", new SSLRResolvePassEffect());
            }
        }
    }
}
