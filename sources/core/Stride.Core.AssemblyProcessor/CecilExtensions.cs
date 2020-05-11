// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mono.Cecil;
using Mono.Cecil.Rocks;

namespace Stride.Core.AssemblyProcessor
{
    // Source: http://stackoverflow.com/questions/4968755/mono-cecil-call-generic-base-class-method-from-other-assembly
    public static class CecilExtensions
    {
        public static bool IsResolvedValueType(this TypeReference type)
        {
            if (type.GetType() == typeof(TypeReference))
                type = type.Resolve();

            return type.IsValueType;
        }

        public static MethodDefinition GetEmptyConstructor(this TypeDefinition type, bool allowPrivate = false)
        {
            return type.Methods.FirstOrDefault(x => x.IsConstructor && (x.IsPublic || allowPrivate) && !x.IsStatic && x.Parameters.Count == 0);
        }

        public static ModuleDefinition GetStrideCoreModule(this AssemblyDefinition assembly)
        {
            var strideCoreAssembly = assembly.Name.Name == "Stride.Core"
                ? assembly
                : assembly.MainModule.AssemblyResolver.Resolve(new AssemblyNameReference("Stride.Core", null));
            var strideCoreModule = strideCoreAssembly.MainModule;
            return strideCoreModule;
        }

        public static void AddModuleInitializer(this MethodDefinition initializeMethod, int order = 0)
        {
            var assembly = initializeMethod.Module.Assembly;
            var strideCoreModule = GetStrideCoreModule(assembly);

            var moduleInitializerAttribute = strideCoreModule.GetType("Stride.Core.ModuleInitializerAttribute");
            var moduleInitializerCtor = moduleInitializerAttribute.GetConstructors().Single(x => !x.IsStatic && x.Parameters.Count == 1);
            initializeMethod.CustomAttributes.Add(
                new CustomAttribute(assembly.MainModule.ImportReference(moduleInitializerCtor))
                {
                    ConstructorArguments = { new CustomAttributeArgument(assembly.MainModule.TypeSystem.Int32, order) }
                });
        }

        public static TypeReference MakeGenericType(this TypeReference self, params TypeReference[] arguments)
        {
            if (self.GenericParameters.Count != arguments.Length)
                throw new ArgumentException();

            if (arguments.Length == 0)
                return self;

            var instance = new GenericInstanceType(self);
            foreach (var argument in arguments)
                instance.GenericArguments.Add(argument);

            return instance;
        }

        public static FieldReference MakeGeneric(this FieldReference self, params TypeReference[] arguments)
        {
            if (arguments.Length == 0)
                return self;

            return new FieldReference(self.Name, self.FieldType, self.DeclaringType.MakeGenericType(arguments));
        }

        public static MethodReference MakeGeneric(this MethodReference self, params TypeReference[] arguments)
        {
            if (arguments.Length == 0)
                return self;

            var reference = new MethodReference(self.Name, self.ReturnType, self.DeclaringType.MakeGenericType(arguments))
            {
                HasThis = self.HasThis,
                ExplicitThis = self.ExplicitThis,
                CallingConvention = self.CallingConvention,
            };

            foreach (var parameter in self.Parameters)
                reference.Parameters.Add(new ParameterDefinition(parameter.ParameterType));

            CopyGenericParameters(self, reference);

            return reference;
        }

        private static void CopyGenericParameters(MethodReference self, MethodReference reference)
        {
            foreach (var genericParameter in self.GenericParameters)
            {
                var genericParameterCopy = new GenericParameter(genericParameter.Name, reference)
                {
                    Attributes = genericParameter.Attributes,
                };
                reference.GenericParameters.Add(genericParameterCopy);
                foreach (var constraint in genericParameter.Constraints)
                    genericParameterCopy.Constraints.Add(constraint);
            }
        }

        public static MethodReference MakeGenericMethod(this MethodReference self, params TypeReference[] arguments)
        {
            if (self.GenericParameters.Count != arguments.Length)
                throw new ArgumentException();

            var method = new GenericInstanceMethod(self);
            foreach(var argument in arguments)
                method.GenericArguments.Add(argument);
            return method;
        }

        public static TypeDefinition GetTypeResolved(this ModuleDefinition moduleDefinition, string typeName)
        {
            foreach (var exportedType in moduleDefinition.ExportedTypes)
            {
                if (exportedType.FullName == typeName)
                {
                    var typeDefinition = exportedType.Resolve();
                    return typeDefinition;
                }
            }

            return moduleDefinition.GetType(typeName);
        }

        /// <summary>
        ///   Finds the Core Library Assembly, which can be either <c>mscorlib.dll</c> or <c>System.Runtime.dll</c>
        ///   depending on the .NET runtime environment.
        /// </summary>
        /// <param name="assembly">Assembly where <see cref="object"/> is found.</param>
        /// <returns>An <see cref="AssemblyDefinition"/> representing the Core Library.</returns>
        public static AssemblyDefinition FindCorlibAssembly(AssemblyDefinition assembly)
        {
            // Ask Cecil for the core library which will be either mscorlib or System.Runtime
            AssemblyNameReference corlibReference = assembly.MainModule.TypeSystem.CoreLibrary as AssemblyNameReference;
            return assembly.MainModule.AssemblyResolver.Resolve(corlibReference);
        }

        /// <summary>
        ///   Finds the assembly in which the generic collections are defined. This can be either in <c>mscorlib.dll</c> or
        ///   in <c>System.Collections.dll</c> depending on the .NET runtime environment.
        /// </summary>
        /// <param name="assembly">Assembly where the generic collections are defined.</param>
        /// <returns>An <see cref="AssemblyDefinition"/> where the generic collections are defined.</returns>
        public static AssemblyDefinition FindCollectionsAssembly(AssemblyDefinition assembly)
        {
            // Ask Cecil for the core library which will be either mscorlib or System.Runtime
            var corlibReference = FindCorlibAssembly(assembly);

            if (corlibReference.Name.Name.ToLower() == "system.runtime")
            {
                // The core library is System.Runtime, so the collections assembly is System.Collections.dll.
                // First we look if it is not already referenced by `assembly' and if not, we made an explicit reference
                // to System.Collections.
                var collectionsAssembly = assembly.MainModule.AssemblyReferences.FirstOrDefault(ass => ass.Name.ToLower() == "system.collections");
                if (collectionsAssembly is null)
                {
                    collectionsAssembly = new AssemblyNameReference("System.Collections", new Version(4,0,0,0));
                }

                return assembly.MainModule.AssemblyResolver.Resolve(collectionsAssembly);
            }
            else
            {
                return corlibReference;
            }
        }

        /// <summary>
        ///   Finds the assembly in which reflection infrastructure is defined. This can be either in <c>mscorlib.dll</c> or
        ///   in <c>System.Collections.dll</c> depending on the .NET runtime environment.
        /// </summary>
        /// <param name="assembly">Assembly where reflection infrastructure is defined.</param>
        /// <returns>An <see cref="AssemblyDefinition"/> where reflection infrastructure is defined.</returns>
        public static AssemblyDefinition FindReflectionAssembly(AssemblyDefinition assembly)
        {
            // Ask Cecil for the core library which will be either mscorlib or System.Runtime
            var corlibReference = FindCorlibAssembly(assembly);

            if (corlibReference.Name.Name.ToLower() == "system.runtime")
            {
                // The core library is System.Runtime, so the reflection infrastructure is in System.Reflection.dll.
                // First we look if it is not already referenced by `assembly' and if not, we made an explicit reference
                // to System.Reflection.
                var reflectionAssembly = assembly.MainModule.AssemblyReferences.FirstOrDefault(ass => ass.Name.ToLower() == "system.reflection");
                if (reflectionAssembly is null)
                {
                    reflectionAssembly = new AssemblyNameReference("System.Reflection", new Version(4, 0, 0, 0));
                }
                return assembly.MainModule.AssemblyResolver.Resolve(reflectionAssembly);
            }
            else
            {
                return corlibReference;
            }
        }

        public static GenericInstanceType ChangeGenericInstanceType(this GenericInstanceType type, TypeReference elementType, IEnumerable<TypeReference> genericArguments)
        {
            if (elementType != type.ElementType || genericArguments != type.GenericArguments)
            {
                var result = new GenericInstanceType(elementType);
                foreach (var genericArgument in genericArguments)
                    result.GenericArguments.Add(genericArgument);
                if (type.HasGenericParameters)
                    SetGenericParameters(result, type.GenericParameters);
                return result;
            }
            return type;
        }

        public static ArrayType ChangeArrayType(this ArrayType type, TypeReference elementType, int rank)
        {
            if (elementType != type.ElementType || rank != type.Rank)
            {
                var result = new ArrayType(elementType, rank);
                if (type.HasGenericParameters)
                    SetGenericParameters(result, type.GenericParameters);
                return result;
            }
            return type;
        }

        public static PointerType ChangePointerType(this PointerType type, TypeReference elementType)
        {
            if (elementType != type.ElementType)
            {
                var result = new PointerType(elementType);
                if (type.HasGenericParameters)
                    SetGenericParameters(result, type.GenericParameters);
                return result;
            }
            return type;
        }

        public static PinnedType ChangePinnedType(this PinnedType type, TypeReference elementType)
        {
            if (elementType != type.ElementType)
            {
                var result = new PinnedType(elementType);
                if (type.HasGenericParameters)
                    SetGenericParameters(result, type.GenericParameters);
                return result;
            }
            return type;
        }

        public static TypeReference ChangeGenericParameters(this TypeReference type, IEnumerable<GenericParameter> genericParameters)
        {
            if (type.GenericParameters == genericParameters)
                return type;

            TypeReference result;
            if (type is ArrayType arrayType)
            {
                result = new ArrayType(arrayType.ElementType, arrayType.Rank);
            }
            else
            {
                if (type is GenericInstanceType genericInstanceType)
                {
                    result = new GenericInstanceType(genericInstanceType.ElementType);
                }
                else if (type.GetType() == typeof(TypeReference).GetType())
                {
                    result = new TypeReference(type.Namespace, type.Name, type.Module, type.Scope, type.IsValueType);
                }
                else
                {
                    throw new NotSupportedException();
                }
            }

            SetGenericParameters(result, genericParameters);

            return result;
        }

        /// <summary>
        ///   Sometimes, <see cref="TypeReference.IsValueType"/> is not properly set (since it needs to load dependent assembly).
        ///   This do so when necessary.
        /// </summary>
        public static TypeReference FixupValueType(this TypeReference typeReference)
        {
            return FixupValueTypeVisitor.Default.VisitDynamic(typeReference);
        }

        private static void SetGenericParameters(TypeReference result, IEnumerable<GenericParameter> genericParameters)
        {
            foreach (var genericParameter in genericParameters)
                result.GenericParameters.Add(genericParameter);
        }

        public static string GenerateGenericsString(this TypeReference type, bool empty = false)
        {
            var genericInstanceType = type as GenericInstanceType;
            if (!type.HasGenericParameters && genericInstanceType is null)
                return string.Empty;

            var result = new StringBuilder();

            // Try to process generic instantiations
            if (genericInstanceType != null)
            {
                result.Append("<");

                bool first = true;
                foreach (var genericArgument in genericInstanceType.GenericArguments)
                {
                    if (!first)
                        result.Append(",");
                    first = false;
                    if (!empty)
                        result.Append(ConvertToValidCSharp(genericArgument, empty));
                }

                result.Append(">");

                return result.ToString();
            }

            if (type.HasGenericParameters)
            {
                result.Append("<");

                bool first = true;
                foreach (var genericParameter in type.GenericParameters)
                {
                    if (!first)
                        result.Append(",");
                    first = false;
                    if (!empty)
                        result.Append(ConvertToValidCSharp(genericParameter, empty));
                }

                result.Append(">");

                return result.ToString();
            }

            return result.ToString();
        }

        /// <summary>
        ///   Generates a type name valid to use from C# source.
        /// </summary>
        public static string ConvertToValidCSharp(this TypeReference type, bool empty = false)
        {
            // Try to process arrays
            if (type is ArrayType arrayType)
            {
                return ConvertToValidCSharp(arrayType.ElementType, empty) + "[]";
            }

            // Remove the `X at end of generic definition.
            var typeName = type.GetElementType().FullName;
            var genericSeparatorIndex = typeName.LastIndexOf('`');
            if (genericSeparatorIndex != -1)
                typeName = typeName.Substring(0, genericSeparatorIndex);

            // Replace / into . (nested types)
            typeName = typeName.Replace('/', '.');

            // Try to process generic instantiations
            if (type is GenericInstanceType genericInstanceType)
            {
                var result = new StringBuilder();

                // Use ElementType so that we have only the name without the <> part.
                result.Append(typeName);
                result.Append("<");

                bool first = true;
                foreach (var genericArgument in genericInstanceType.GenericArguments)
                {
                    if (!first)
                        result.Append(",");
                    first = false;
                    if (!empty)
                        result.Append(ConvertToValidCSharp(genericArgument, empty));
                }

                result.Append(">");

                return result.ToString();
            }

            if (type.HasGenericParameters)
            {
                var result = new StringBuilder();

                // Use ElementType so that we have only the name without the <> part.
                result.Append(typeName);
                result.Append("<");

                bool first = true;
                foreach (var genericParameter in type.GenericParameters)
                {
                    if (!first)
                        result.Append(",");
                    first = false;
                    if (!empty)
                        result.Append(ConvertToValidCSharp(genericParameter, empty));
                }

                result.Append(">");

                return result.ToString();
            }

            return typeName;
        }

        /// <summary>
        ///   Generates the <c>Mono.Cecil</c> <see cref="TypeReference"/> from its .NET <see cref="Type"/> counterpart.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="assemblyResolver">The assembly resolver.</param>
        /// <returns><see cref="TypeReference"/> for the provided type.</returns>
        public static TypeReference GenerateTypeCecil(this Type type, BaseAssemblyResolver assemblyResolver)
        {
            var assemblyDefinition = assemblyResolver.Resolve(AssemblyNameReference.Parse(type.Assembly.FullName));
            TypeReference typeReference;

            if (type.IsNested)
            {
                var declaringType = GenerateTypeCecil(type.DeclaringType, assemblyResolver);
                typeReference = declaringType.Resolve().NestedTypes.FirstOrDefault(x => x.Name == type.Name);
            }
            else if (type.IsArray)
            {
                var elementType = GenerateTypeCecil(type.GetElementType(), assemblyResolver);
                typeReference = new ArrayType(elementType, type.GetArrayRank());
            }
            else
            {
                typeReference = assemblyDefinition.MainModule.GetTypeResolved(type.IsGenericType ? type.GetGenericTypeDefinition().FullName : type.FullName);
            }

            if (typeReference == null)
                throw new InvalidOperationException("Could not resolve Mono.Cecil type.");

            if (type.IsGenericType)
            {
                var genericInstanceType = new GenericInstanceType(typeReference);
                foreach (var argType in type.GetGenericArguments())
                {
                    TypeReference argTypeReference;
                    if (argType.IsGenericParameter)
                    {
                        argTypeReference = new GenericParameter(argType.Name, typeReference);
                    }
                    else
                    {
                        argTypeReference = GenerateTypeCecil(argType, assemblyResolver);
                    }
                    genericInstanceType.GenericArguments.Add(argTypeReference);
                }

                typeReference = genericInstanceType;
            }

            return typeReference;
        }

        /// <summary>
        ///   Generates a type name similar to <see cref="Type.AssemblyQualifiedName"/>.
        /// </summary>
        /// <param name="type">The <see cref="TypeReference"/>.</param>
        /// <returns>Type name similar to <see cref="Type.AssemblyQualifiedName"/>.</returns>
        public static string ConvertAssemblyQualifiedName(this TypeReference type)
        {
            var result = new StringBuilder(256);
            ConvertAssemblyQualifiedName(type, result);
            return result.ToString();
        }

        private static void ConvertAssemblyQualifiedName(this TypeReference type, StringBuilder result)
        {
            var arrayType = type as ArrayType;
            if (arrayType != null)
            {
                // If it's an array, process element type, and add [] after
                type = arrayType.ElementType;
            }

            // Add FUllName from GetElementType() (remove generics etc...)
            int start = result.Length;
            result.Append(type.GetElementType().FullName);
            int end = result.Length;

            // Replace / into + (nested types)
            result = result.Replace('/', '+', start, end);

            // Try to process generic instantiations
            if (type is GenericInstanceType genericInstanceType)
            {
                if (!type.ContainsGenericParameter)
                {
                    // Use ElementType so that we have only the name without the <> part.
                    result.Append('[');

                    bool first = true;
                    foreach (var genericArgument in genericInstanceType.GenericArguments)
                    {
                        if (!first)
                            result.Append(",");
                        result.Append('[');
                        first = false;
                        result.Append(ConvertAssemblyQualifiedName(genericArgument));
                        result.Append(']');
                    }

                    result.Append(']');
                }
            }

            // Try to process arrays
            if (arrayType != null)
            {
                result.Append('[');
                if (arrayType.Rank > 1)
                    result.Append(',', arrayType.Rank - 1);
                result.Append(']');
            }

            result.Append(", ");
            result.Append(type.Module.Assembly.FullName);
        }

        public static CustomAttribute GetCustomAttribute<T>(this AssemblyDefinition assembly) where T : Attribute
        {
            return assembly.CustomAttributes.FirstOrDefault(
                x => string.Compare(x.AttributeType.FullName, typeof(T).FullName, StringComparison.Ordinal) == 0);
        }

        public static CustomAttribute GetCustomAttribute(this AssemblyDefinition assembly, string attributeFullName)
        {
            return assembly.CustomAttributes.FirstOrDefault(
                x => string.Compare(x.AttributeType.FullName, attributeFullName, StringComparison.Ordinal) == 0);
        }

        public static CustomAttribute GetCustomAttribute<T>(this TypeDefinition type) where T : Attribute
        {
            return type.CustomAttributes.FirstOrDefault(
                x => string.Compare(x.AttributeType.FullName, typeof(T).FullName, StringComparison.Ordinal) == 0);
        }

        public static CustomAttribute GetCustomAttribute(this TypeDefinition type, string attributeFullName)
        {
            return type.CustomAttributes.FirstOrDefault(
                x => string.Compare(x.AttributeType.FullName, attributeFullName, StringComparison.Ordinal) == 0);
        }

        public static CustomAttribute GetCustomAttribute<T>(this FieldDefinition field) where T : Attribute
        {
            return field.CustomAttributes.FirstOrDefault(
                x => string.Compare(x.AttributeType.FullName, typeof(T).FullName, StringComparison.Ordinal) == 0);
        }

        public static CustomAttribute GetCustomAttribute(this FieldDefinition field, string attributeFullName)
        {
            return field.CustomAttributes.FirstOrDefault(
                x => string.Compare(x.AttributeType.FullName, attributeFullName, StringComparison.Ordinal) == 0);
        }

        public static bool HasCustomAttribute(this AssemblyDefinition assembly, string attributeFullName)
        {
            return assembly.CustomAttributes.Any(
                x => string.Compare(x.AttributeType.FullName, attributeFullName, StringComparison.Ordinal) == 0);
        }

        public static bool HasCustomAttribute(this FieldDefinition field, string attributeFullName)
        {
            return field.CustomAttributes.Any(
                x => string.Compare(x.AttributeType.FullName, attributeFullName, StringComparison.Ordinal) == 0);
        }

        public static bool HasCustomAttribute(this PropertyDefinition property, string attributeFullName)
        {
            return property.CustomAttributes.Any(
                x => string.Compare(x.AttributeType.FullName, attributeFullName, StringComparison.Ordinal) == 0);
        }
    }
}
