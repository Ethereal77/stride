﻿// <auto-generated>
// Do not edit this file yourself!
//
// This code was generated by Stride Shader Mixin Code Generator.
// To generate it yourself, please install Stride.VisualStudio.Package .vsix
// and re-save the associated .sdfx.
// </auto-generated>

using System;

using Stride.Core;
using Stride.Rendering;
using Stride.Graphics;
using Stride.Shaders;
using Stride.Core.Mathematics;
using Buffer = Stride.Graphics.Buffer;

namespace Stride.Rendering
{
    public static partial class TransformationKeys
    {
        public static readonly ValueParameterKey<Matrix> View = ParameterKeys.NewValue<Matrix>();
        public static readonly ValueParameterKey<Matrix> ViewInverse = ParameterKeys.NewValue<Matrix>();
        public static readonly ValueParameterKey<Matrix> Projection = ParameterKeys.NewValue<Matrix>();
        public static readonly ValueParameterKey<Matrix> ProjectionInverse = ParameterKeys.NewValue<Matrix>();
        public static readonly ValueParameterKey<Matrix> ViewProjection = ParameterKeys.NewValue<Matrix>();
        public static readonly ValueParameterKey<Vector2> ProjScreenRay = ParameterKeys.NewValue<Vector2>();
        public static readonly ValueParameterKey<Vector4> Eye = ParameterKeys.NewValue<Vector4>();
        public static readonly ValueParameterKey<Matrix> World = ParameterKeys.NewValue<Matrix>();
        public static readonly ValueParameterKey<Matrix> WorldInverse = ParameterKeys.NewValue<Matrix>();
        public static readonly ValueParameterKey<Matrix> WorldInverseTranspose = ParameterKeys.NewValue<Matrix>();
        public static readonly ValueParameterKey<Matrix> WorldView = ParameterKeys.NewValue<Matrix>();
        public static readonly ValueParameterKey<Matrix> WorldViewInverse = ParameterKeys.NewValue<Matrix>();
        public static readonly ValueParameterKey<Matrix> WorldViewProjection = ParameterKeys.NewValue<Matrix>();
        public static readonly ValueParameterKey<Vector3> WorldScale = ParameterKeys.NewValue<Vector3>();
        public static readonly ValueParameterKey<Vector4> EyeMS = ParameterKeys.NewValue<Vector4>();
    }
}
