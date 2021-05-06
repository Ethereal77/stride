// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Stride.Core.Assets;

namespace Stride.Core.Serialization
{
    /// <summary>
    ///   Helper class for <see cref="UrlReference"/> and <see cref="UrlReference{T}"/>.
    /// </summary>
    public static class UrlReferenceHelper
    {
        /// <summary>
        ///   Creates a <see cref?"IUrlReference"/> to the given Asset that matches the given reference type.
        /// </summary>
        /// <param name="referenceType">The type of reference to create.</param>
        /// <param name="assetId">The target Asset id to create.</param>
        /// <param name="assetUrl">The target Asset URL to create.</param>
        /// <returns>
        ///   A <see cref?"IUrlReference"/> to the given Asset if it's not <c>null</c> and <paramref name="referenceType"/>
        ///   is a valid reference URL type; <c>null</c> otherwise.
        /// </returns>
        /// <remarks>A reference type is either an <see cref="UrlReference"/> or a <see cref="UrlReference{T}"/>.</remarks>
        public static object CreateReference(Type referenceType, AssetId assetId, string assetUrl)
        {
            if (assetId != AssetId.Empty && assetUrl != null && IsUrlReferenceType(referenceType))
            {
                var urlReference = (UrlReferenceBase) AttachedReferenceManager.CreateProxyObject(referenceType, assetId, assetUrl);
                urlReference.Url = assetUrl;
                return urlReference;
            }

            return null;
        }

        /// <summary>
        ///   Checks if the given type is either an <see cref="UrlReference"/> or a <see cref="UrlReference{T}"/>.
        /// </summary>
        /// <param name="type">The type to test.</param>
        /// <returns><c>true</c> if the <paramref name="type"/> specified is an URL reference; <c>false</c> otherwise.</returns>
        public static bool IsUrlReferenceType(Type type)
            => type != null && typeof(UrlReference).IsAssignableFrom(type) || IsGenericUrlReferenceType(type);

        /// <summary>
        ///   Checks if the given type is a <see cref="UrlReference{T}"/>
        /// </summary>
        /// <param name="type">The type to test.</param>
        /// <returns><c>true</c> if the <paramref name="type"/> specified is an URL reference; <c>false</c> otherwise.</returns>
        public static bool IsGenericUrlReferenceType(Type type)
            => type != null && IsSubclassOfRawGeneric(GenericType, type);

        /// <summary>
        ///   Gets the Asset Content type for a given URL reference type.
        /// </summary>
        /// <param name="type">The type of an URL reference, either an <see cref="UrlReference"/> or a <see cref="UrlReference{T}"/>.</param>
        /// <returns>The target Content type, or <c>null</c> if not a valid URL reference type.</returns>
        public static Type GetTargetContentType(Type type)
        {
            if (!IsUrlReferenceType(type))
                return null;

            if (IsSubclassOfRawGeneric(GenericType, type))
                return type.GetGenericArguments()[0];

            return null;
        }

        private static readonly Type GenericType = typeof(UrlReference<>);

        // TODO: This should probably be put in one of the Reflection helper classes.
        static bool IsSubclassOfRawGeneric(Type type, Type c)
        {
            while (c != null && c != typeof(object))
            {
                var cur = c.IsGenericType ? c.GetGenericTypeDefinition() : c;
                if (type == cur)
                {
                    return true;
                }
                c = c.BaseType;
            }
            return false;
        }
    }
}
