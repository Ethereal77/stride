// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Runtime.InteropServices;

using Stride.Core.IO;

namespace Stride.Core.Storage
{
    /// <summary>
    ///   Represents an immutable binary content data object.
    /// </summary>
    public class Blob : ReferenceBase
    {
        /// <summary>
        ///   Gets the size of the data blob.
        /// </summary>
        /// <value>Size of the data blob, in bytes.</value>
        public int Size { get; }

        /// <summary>
        ///   Gets an <see cref="IntPtr"/> pointing to the blob data.
        /// </summary>
        /// <value>The pointer to the content.</value>
        public IntPtr Content { get; }

        /// <summary>
        ///   Gets the <see cref="Storage.ObjectId"/> that uniquely identifies this blob.
        /// </summary>
        /// <value>The <see cref="Storage.ObjectId"/> of this blob.</value>
        public ObjectId ObjectId { get; }

        /// <summary>
        ///   Gets the object database where this blob is stored.
        /// </summary>
        /// <value>The <see cref="Storage.ObjectDatabase"/> where the blob is stored.</value>
        internal ObjectDatabase ObjectDatabase { get; }


        /// <summary>
        ///   Initializes a new instance of the <see cref="Blob"/> class.
        /// </summary>
        /// <param name="objectDatabase">Object database that stores this blob.</param>
        /// <param name="objectId">Hash that identifies the data.</param>
        protected Blob(ObjectDatabase objectDatabase, ObjectId objectId)
        {
            ObjectDatabase = objectDatabase;
            ObjectId = objectId;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="Blob"/> class.
        /// </summary>
        /// <param name="objectDatabase">Object database that stores this blob.</param>
        /// <param name="objectId">Hash that identifies the data.</param>
        /// <param name="content">Pointer to the data to store.</param>
        /// <param name="size">Size of the data, in bytes.</param>
        internal Blob(ObjectDatabase objectDatabase, ObjectId objectId, IntPtr content, int size)
            : this(objectDatabase, objectId)
        {
            Size = size;
            Content = Marshal.AllocHGlobal(size);

            Utilities.CopyMemory(dest: Content, content, size);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="Blob"/> class.
        /// </summary>
        /// <param name="objectDatabase">Object database that stores this blob.</param>
        /// <param name="objectId">Hash that identifies the data.</param>
        /// <param name="stream">A <see cref="NativeStream"/> with the data to store.</param>
        internal Blob(ObjectDatabase objectDatabase, ObjectId objectId, NativeStream stream)
            : this(objectDatabase, objectId)
        {
            Size = (int) stream.Length;
            Content = Marshal.AllocHGlobal(Size);

            stream.Read(Content, Size);
        }


        /// <summary>
        ///   Returns a <see cref="NativeStream"/> that gives access to the <see cref="Content"/>.
        /// </summary>
        /// <returns>
        ///   A <see cref="NativeStream"/> over the <see cref="Content"/> data. Note that the returned stream will keep a
        ///   reference to the <see cref="Blob"/> until disposed.
        /// </returns>
        public NativeStream GetContentStream()
        {
            return new BlobStream(this);
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            ObjectDatabase.DestroyBlob(this);
            Marshal.FreeHGlobal(Content);
        }
    }
}
