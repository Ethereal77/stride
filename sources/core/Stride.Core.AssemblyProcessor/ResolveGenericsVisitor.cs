// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;

using Mono.Cecil;

namespace Stride.Core.AssemblyProcessor
{
    /// <summary>
    ///   Transforms open generic types to closed instantiation using context information.
    ///   As an example, if <c>B{T}</c> inherits from <c>A{T}</c>, running it with <c>B{C}</c> as context and <c>A{B.T}</c>
    ///   as type, it will return <c>A{C}</c>.
    /// </summary>
    class ResolveGenericsVisitor : CecilTypeReferenceVisitor
    {
        private Dictionary<TypeReference, TypeReference> genericTypeMapping;

        public ResolveGenericsVisitor(Dictionary<TypeReference, TypeReference> genericTypeMapping)
        {
            this.genericTypeMapping = genericTypeMapping;
        }

        /// <summary>
        ///   Transforms open generic types to closed instantiation using context information.
        ///   As an example, if <c>B{T}</c> inherits from <c>A{T}</c>, running it with <c>B{C}</c> as context and <c>A{B.T}</c>
        ///   as type, it will return <c>A{C}</c>.
        /// </summary>
        public static TypeReference Process(TypeReference context, TypeReference type)
        {
            if (type is null)
                return null;

            var parentContext = context;
            GenericInstanceType genericInstanceTypeContext = null;
            while (parentContext != null)
            {
                genericInstanceTypeContext = parentContext as GenericInstanceType;
                if (genericInstanceTypeContext != null)
                    break;

                parentContext = parentContext.Resolve().BaseType;
            }

            if (genericInstanceTypeContext is null || genericInstanceTypeContext.ContainsGenericParameter)
                return type;

            // Build dictionary that will map generic type to their real implementation type
            var genericTypeMapping = new Dictionary<TypeReference, TypeReference>();
            while (parentContext != null)
            {
                var resolvedType = parentContext.Resolve();
                for (int i = 0; i < resolvedType.GenericParameters.Count; ++i)
                {
                    var genericParameter = parentContext.GetElementType().Resolve().GenericParameters[i];
                    genericTypeMapping.Add(genericParameter, genericInstanceTypeContext.GenericArguments[i]);
                }
                parentContext = parentContext.Resolve().BaseType;
                if (parentContext is GenericInstanceType)
                    genericInstanceTypeContext = parentContext as GenericInstanceType;
            }

            var visitor = new ResolveGenericsVisitor(genericTypeMapping);
            var result = visitor.VisitDynamic(type);

            // Make sure type is closed now
            if (result.ContainsGenericParameter)
                throw new InvalidOperationException("Unsupported generic resolution.");

            return result;
        }

        public override TypeReference Visit(GenericParameter type)
        {
            TypeReference typeParent = type;

            while (genericTypeMapping.TryGetValue(typeParent, out TypeReference result))
                typeParent = result;

            if (typeParent != type)
                return typeParent;

            return base.Visit(type);
        }
    }
}
