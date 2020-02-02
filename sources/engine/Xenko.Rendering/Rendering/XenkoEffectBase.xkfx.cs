﻿// <auto-generated>
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

using Xenko.Rendering.Data;
using Xenko.Rendering.Materials;
namespace Xenko.Rendering
{
    internal static partial class ShaderMixins
    {
        internal partial class XenkoEffectBase  : IShaderMixinBuilder
        {
            public void Generate(ShaderMixinSource mixin, ShaderMixinContext context)
            {
                context.Mixin(mixin, "ShaderBase");
                context.Mixin(mixin, "ShadingBase");
                var extensionPreVertexStageSurfaceShaders = context.GetParam(MaterialKeys.VertexStageSurfaceShaders);
                if (extensionPreVertexStageSurfaceShaders != null)
                {
                    context.Mixin(mixin, "MaterialSurfaceVertexStageCompositor");

                    {
                        var __mixinToCompose__ = (extensionPreVertexStageSurfaceShaders);
                        var __subMixin = new ShaderMixinSource();
                        context.PushComposition(mixin, "materialVertexStage", __subMixin);
                        context.Mixin(__subMixin, __mixinToCompose__);
                        context.PopComposition();
                    }

                    {
                        var __mixinToCompose__ = context.GetParam(MaterialKeys.VertexStageStreamInitializer);
                        var __subMixin = new ShaderMixinSource();
                        context.PushComposition(mixin, "streamInitializerVertexStage", __subMixin);
                        context.Mixin(__subMixin, __mixinToCompose__);
                        context.PopComposition();
                    }
                }
                context.Mixin(mixin, "TransformationBase");
                context.Mixin(mixin, "NormalStream");
                context.Mixin(mixin, "TransformationWAndVP");
                if (context.GetParam(MaterialKeys.HasNormalMap))
                {
                    context.Mixin(mixin, "NormalFromNormalMapping");
                }
                else
                {
                    context.Mixin(mixin, "NormalFromMesh");
                }
                if (context.GetParam(MaterialKeys.HasSkinningPosition))
                {
                    mixin.AddMacro("SkinningMaxBones", context.GetParam(MaterialKeys.SkinningMaxBones));
                    context.Mixin(mixin, "TransformationSkinning");
                    if (context.GetParam(MaterialKeys.HasSkinningNormal))
                    {
                        context.Mixin(mixin, "NormalMeshSkinning");
                    }
                    if (context.GetParam(MaterialKeys.HasSkinningTangent))
                    {
                        context.Mixin(mixin, "TangentMeshSkinning");
                    }
                    if (context.GetParam(MaterialKeys.HasSkinningNormal))
                    {
                        if (context.GetParam(MaterialKeys.HasNormalMap))
                        {
                            context.Mixin(mixin, "NormalVSSkinningNormalMapping");
                        }
                        else
                        {
                            context.Mixin(mixin, "NormalVSSkinningFromMesh");
                        }
                    }
                }
                var extensionTessellationShader = context.GetParam(MaterialKeys.TessellationShader);
                if (extensionTessellationShader != null)
                {
                    context.Mixin(mixin, (extensionTessellationShader));
                    var extensionDomainStageSurfaceShaders = context.GetParam(MaterialKeys.DomainStageSurfaceShaders);
                    if (extensionDomainStageSurfaceShaders != null)
                    {
                        context.Mixin(mixin, "MaterialSurfaceDomainStageCompositor");

                        {
                            var __mixinToCompose__ = (extensionDomainStageSurfaceShaders);
                            var __subMixin = new ShaderMixinSource();
                            context.PushComposition(mixin, "materialDomainStage", __subMixin);
                            context.Mixin(__subMixin, __mixinToCompose__);
                            context.PopComposition();
                        }

                        {
                            var __mixinToCompose__ = context.GetParam(MaterialKeys.DomainStageStreamInitializer);
                            var __subMixin = new ShaderMixinSource();
                            context.PushComposition(mixin, "streamInitializerDomainStage", __subMixin);
                            context.Mixin(__subMixin, __mixinToCompose__);
                            context.PopComposition();
                        }
                    }
                }
                var computeVelocityShader = context.GetParam(XenkoEffectBaseKeys.ComputeVelocityShader);
                if (computeVelocityShader != null)
                {
                    context.Mixin(mixin, (computeVelocityShader));
                }
                var extensionPostVertexStage = context.GetParam(XenkoEffectBaseKeys.ExtensionPostVertexStageShader);
                if (extensionPostVertexStage != null)
                {
                    context.Mixin(mixin, (extensionPostVertexStage));
                }
                var targetExtensions = context.GetParam(XenkoEffectBaseKeys.RenderTargetExtensions);
                if (targetExtensions != null)
                {
                    context.Mixin(mixin, (targetExtensions));
                }
            }

            [ModuleInitializer]
            internal static void __Initialize__()

            {
                ShaderMixinManager.Register("XenkoEffectBase", new XenkoEffectBase());
            }
        }
    }
}
