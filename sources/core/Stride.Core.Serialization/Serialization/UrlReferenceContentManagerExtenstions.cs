// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.IO;
using System.Threading.Tasks;

using Stride.Core.IO;
using Stride.Core.Serialization.Contents;

namespace Stride.Core.Serialization
{
    /// <summary>
    ///   Extension methods for <see cref="IContentManager"/> to allow the use of <see cref="UrlReference"/> and <see cref="UrlReference{T}"/>.
    /// </summary>
    public static class UrlReferenceContentManagerExtenstions
    {
        /// <summary>
        ///   Determines whether the specified Asset URL exists.
        /// </summary>
        /// <param name="content">The <see cref="IContentManager"/>.</param>
        /// <param name="urlReference">The URL to check.</param>
        /// <returns><c>true</c> if the specified Asset URL <paramref name="urlReference"/> exists; <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="urlReference"/> is <c>null</c> or empty.
        ///   Or
        ///   <paramref name="content"/> is <c>null</c>.
        /// </exception>
        public static bool Exists(this IContentManager content, IUrlReference urlReference)
        {
            CheckArguments(content, urlReference);

            return content.Exists(urlReference.Url);
        }

        /// <summary>
        ///   Opens the specified URL as a <see cref="Stream"/> for custom raw Asset loading.
        /// </summary>
        /// <param name="content">The <see cref="IContentManager"/>.</param>
        /// <param name="urlReference">The URL to the raw Asset.</param>
        /// <param name="streamFlags">One or more values of <see cref="StreamFlags"/> indicating the type of stream needed.</param>
        /// <returns>A <see cref="Stream"/> to the raw asset.</returns>
        /// <exception cref="ArgumentNullException">
        ///    <paramref name="urlReference"/> is <c>null</c> or empty.
        ///    Or
        ///    <paramref name="content"/> is <c>null</c>.
        ///  </exception>
        public static Stream OpenAsStream(this IContentManager content, UrlReference urlReference, StreamFlags streamFlags = StreamFlags.None)
        {
            CheckArguments(content, urlReference);

            return content.OpenAsStream(urlReference.Url, streamFlags);
        }

        /// <summary>
        ///   Loads content from the specified URL.
        /// </summary>
        /// <typeparam name="T">Type of the content to load.</typeparam>
        /// <param name="content">The <see cref="IContentManager"/>.</param>
        /// <param name="urlReference">The URL to load from.</param>
        /// <param name="settings">The settings. If <c>null</c>, <see cref="ContentManagerLoaderSettings.Default"/> will be used.</param>
        /// <returns>The loaded content.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="urlReference"/> is <c>null</c> or empty.
        ///   Or
        ///   <paramref name="content"/> is <c>null</c>.
        /// </exception>
        public static T Load<T>(this IContentManager content, UrlReference<T> urlReference, ContentManagerLoaderSettings settings = null)
            where T : class
        {
            CheckArguments(content, urlReference);

            return content.Load<T>(urlReference.Url, settings);
        }

        /// <summary>
        ///   Loads content from the specified URL asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of the content to load.</typeparam>
        /// <param name="content">The <see cref="IContentManager"/>.</param>
        /// <param name="urlReference">The URL to load from.</param>
        /// <param name="settings">The settings. If <c>null</c>, <see cref="ContentManagerLoaderSettings.Default"/> will be used.</param>
        /// <returns>The loaded content.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="urlReference"/> is <c>null</c> or empty.
        ///   Or
        ///   <paramref name="content"/> is <c>null</c>.
        /// </exception>
        public static Task<T> LoadAsync<T>(this IContentManager content, UrlReference<T> urlReference, ContentManagerLoaderSettings settings = null)
            where T : class
        {
            CheckArguments(content, urlReference);

            return content.LoadAsync<T>(urlReference.Url, settings);
        }

        /// <summary>
        ///   Gets a previously loaded Asset from its URL.
        /// </summary>
        /// <typeparam name="T">The type of Asset to retrieve.</typeparam>
        /// <param name="urlReference">The URL of the asset to retrieve.</param>
        /// <returns>The loaded Asset, or <c>null</c> if the Asset has not been loaded.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="urlReference"/> is <c>null</c> or empty.
        ///   Or
        ///   <paramref name="content"/> is <c>null</c>.
        /// </exception>
        /// <remarks>This function does not increase the reference count on the Asset.</remarks>
        public static T Get<T>(this IContentManager content, UrlReference<T> urlReference)
            where T : class
        {
            CheckArguments(content, urlReference);

            return content.Get<T>(urlReference.Url);
        }

        /// <summary>
        ///   Gets whether an Asset with the given URL is currently loaded.
        /// </summary>
        /// <param name="urlReference">The URL to check.</param>
        /// <param name="loadedManuallyOnly">
        ///   If <c>true</c>, this method will return <c>true</c> only if an Asset with the given URL has been manually
        ///   loaded via <see cref="Load"/>, and not if the Asset has been only loaded indirectly from another Asset.
        /// </param>
        /// <returns><c>true</c> if an asset with the given URL is currently loaded; <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="urlReference"/> is <c>null</c> or empty.
        ///   Or
        ///   <paramref name="content"/> is <c>null</c>.
        /// </exception>
        public static bool IsLoaded(this IContentManager content, IUrlReference urlReference, bool loadedManuallyOnly = false)
        {
            CheckArguments(content, urlReference);

            return content.IsLoaded(urlReference.Url, loadedManuallyOnly);
        }

        /// <summary>
        ///   Unloads the Asset with the specified URL.
        /// </summary>
        /// <param name="urlReference">The URL of the Asset to unload.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="urlReference"/> is <c>null</c> or empty.
        ///   Or
        ///   <paramref name="content"/> is <c>null</c>.
        /// </exception>
        public static void Unload(this IContentManager content, IUrlReference urlReference)
        {
            CheckArguments(content, urlReference);

            content.Unload(urlReference.Url);
        }

        private static void CheckArguments(IContentManager content, IUrlReference urlReference)
        {
            if (content is null)
                throw new ArgumentNullException(nameof(content));

            if (urlReference is null || urlReference.IsEmpty)
                throw new ArgumentNullException(nameof(urlReference));
        }
    }
}
