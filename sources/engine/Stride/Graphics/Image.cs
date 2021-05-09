// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// See the LICENSE.md file in the project root for full license information.

// -----------------------------------------------------------------------------
// Part of te following code is a port of http://directxtex.codeplex.com
// Licensed under Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

using Stride.Core;
using Stride.Core.Serialization.Contents;

namespace Stride.Graphics
{
    /// <summary>
    ///   Represents a 1D/2D/3D image with support for TextureArray and mipmaps on the CPU.
    ///   Provides methods to create new inages and to load/save an image from the disk.
    /// </summary>
    [ContentSerializer(typeof(ImageSerializer))]
    public sealed class Image : IDisposable
    {
        public delegate Image ImageLoadDelegate(IntPtr dataPointer, int dataSize, bool makeACopy, GCHandle? handle);

        public delegate void ImageSaveDelegate(PixelBuffer[] pixelBuffers, int count, ImageDescription description, Stream imageStream);

        private const string MagicCodeTKTX = "TKTX";

        /// <summary>
        ///   Pixel buffers.
        /// </summary>
        internal PixelBuffer[] PixelBuffers;

        private DataBox[] dataBoxArray;
        private List<int> mipMapToZIndex;
        private int zBufferCountPerArraySlice;
        private MipMapDescription[] mipmapDescriptions;

        private static List<LoadSaveDelegate> loadSaveDelegates = new List<LoadSaveDelegate>();

        /// <summary>
        ///   Provides access to all pixel buffers.
        /// </summary>
        /// <remarks>
        ///   For Texture3D, each Z slice of the Texture3D has a pixelBufferArray for each one of the mipmaps.
        ///   For other textures, there is Description.MipLevels * Description.ArraySize pixel buffers.
        /// </remarks>
        private PixelBufferArray pixelBufferArray;

        /// <summary>
        ///   Total number of bytes occupied by this image in memory.
        /// </summary>
        private int totalSizeInBytes;

        /// <summary>
        ///   Pointer to the buffer.
        /// </summary>
        private IntPtr buffer;

        /// <summary>
        ///   Indicates if the buffer must be disposed.
        /// </summary>
        private bool bufferIsDisposable;

        /// <summary>
        ///   A <see cref="GCHandle"/> used when the buffer is a pinned managed object on the LOH (Large Object Heap).
        /// </summary>
        private GCHandle? handle;

        /// <summary>
        ///   Description of this image.
        /// </summary>
        public ImageDescription Description;


        /// <summary>
        ///   Converts the format of the description and the pixel buffers to sRGB.
        /// </summary>
        public void ConvertFormatToSRgb()
        {
            Description.Format = Description.Format.ToSRgb();
            if (PixelBuffers != null)
                foreach (var pixelBuffer in PixelBuffers)
                    pixelBuffer.ConvertFormatToSRgb();
        }

        /// <summary>
        ///   Converts the format of the description and the pixel buffers to non-sRGB.
        /// </summary>
        public void ConvertFormatToNonSRgb()
        {
            Description.Format = Description.Format.ToNonSRgb();
            if (PixelBuffers != null)
                foreach (var pixelBuffer in PixelBuffers)
                    pixelBuffer.ConvertFormatToNonSRgb();
        }

        internal Image() { }

        /// <summary>
        ///   Initializes a new instance of the <see cref="Image" /> class.
        /// </summary>
        /// <param name="description">The image description.</param>
        /// <param name="dataPointer">The pointer to the data buffer.</param>
        /// <param name="offset">The offset from the beginning of the data buffer.</param>
        /// <param name="handle">The handle for when the buffer is allocated as a pinned buffer in the LOH (optionnal).</param>
        /// <param name="bufferIsDisposable"><c>true</c> if the buffer should be disposed.</param>
        /// <exception cref="InvalidOperationException">
        ///   If the format is invalid, or width/height/depth/arraysize is invalid with respect to the dimension.
        /// </exception>
        internal unsafe Image(ImageDescription description, IntPtr dataPointer, int offset, GCHandle? handle, bool bufferIsDisposable, PitchFlags pitchFlags = PitchFlags.None, int rowStride = 0)
        {
            Initialize(description, dataPointer, offset, handle, bufferIsDisposable, pitchFlags, rowStride);
        }

        /// <summary>
        ///   Frees the buffers and resources associated to this instance.
        /// </summary>
        public void Dispose()
        {
            if (handle.HasValue)
                handle.Value.Free();

            if (bufferIsDisposable)
                Utilities.FreeMemory(buffer);
        }

        /// <summary>
        ///   Clears the contents of the buffer (zeroes the buffer).
        /// </summary>
        /// <remarks>
        ///   By default, the buffer of a new image is not cleared. Use this method to reset it.
        /// </remarks>
        public void Clear()
        {
            Utilities.ClearMemory(buffer, 0, totalSizeInBytes);
        }

        /// <summary>
        ///   Gets a description of a mipmap of this instance for the specified mipmap level.
        /// </summary>
        /// <param name="mipmap">The mipmap level.</param>
        /// <returns>A <see cref="MipMapDescription"/> of the specified mipmap.</returns>
        public MipMapDescription GetMipMapDescription(int mipmap)
        {
            return mipmapDescriptions[mipmap];
        }

        /// <summary>
        ///   Gets the pixel buffer for the specified array index/Z-slice and mipmap level.
        /// </summary>
        /// <param name="arrayOrZSliceIndex">
        ///   For a 3D image, the parameter is the Z slice, otherwise it is an index into the texture array.
        /// </param>
        /// <param name="mipmap">The mipmap level.</param>
        /// <returns>A <see cref="Graphics.PixelBuffer"/>.</returns>
        /// <exception cref="ArgumentException">
        ///   If <paramref name="arrayOrZSliceIndex"/> or <paramref name="mipmap"/> are out of range.
        /// </exception>
        public PixelBuffer GetPixelBuffer(int arrayOrZSliceIndex, int mipmap)
        {
            if (mipmap > Description.MipLevels)
                throw new ArgumentException("Invalid mipmap level.", nameof(mipmap));

            if (Description.Dimension == TextureDimension.Texture3D)
            {
                if (arrayOrZSliceIndex > Description.Depth)
                    throw new ArgumentException("Invalid Z slice index.", nameof(arrayOrZSliceIndex));

                // For 3D textures
                return GetPixelBufferUnsafe(0, arrayOrZSliceIndex, mipmap);
            }

            if (arrayOrZSliceIndex > Description.ArraySize)
                throw new ArgumentException("Invalid array index.", nameof(arrayOrZSliceIndex));

            // For 1D, 2D textures
            return GetPixelBufferUnsafe(arrayOrZSliceIndex, 0, mipmap);
        }

        /// <summary>
        ///   Gets the pixel buffer for the specified array index/Z-slice and mipmap level.
        /// </summary>
        /// <param name="arrayIndex">Index into the texture array. Must be 0 for 3D images.</param>
        /// <param name="zIndex">Z slice of the 3D image. Must be 0 for 1D or 2D images.</param>
        /// <param name="mipmap">The mipmap level.</param>
        /// <returns>A <see cref="Graphics.PixelBuffer"/>.</returns>
        /// <exception cref="ArgumentException">
        ///   If <paramref name="arrayIndex"/>, <paramref name="zIndex"/> or <paramref name="mipmap"/> are out of range.
        /// </exception>
        public PixelBuffer GetPixelBuffer(int arrayIndex, int zIndex, int mipmap)
        {
            if (mipmap > Description.MipLevels)
                throw new ArgumentException("Invalid mipmap level.", nameof(mipmap));

            if (arrayIndex > Description.ArraySize)
                throw new ArgumentException("Invalid array index.", nameof(arrayIndex));

            if (zIndex > Description.Depth)
                throw new ArgumentException("Invalid Z slice index.", nameof(zIndex));

            return GetPixelBufferUnsafe(arrayIndex, zIndex, mipmap);
        }

        /// <summary>
        ///   Registers a loader / saver for a specified image file type.
        /// </summary>
        /// <param name="type">
        ///   The file type. You can use an integer value as a <see cref="ImageFileType"/> to register other file formats.
        /// </param>
        /// <param name="loader">The loader delegate. Specify <c>null</c> to not register a specific loader.</param>
        /// <param name="saver">The saver delegate. Specify <c>null</c> to not register a specific saver.</param>
        /// <exception cref="ArgumentNullException">Cannot set both loader and saver to <c>null</c>.</exception>
        public static void Register(ImageFileType type, ImageLoadDelegate loader, ImageSaveDelegate saver)
        {
            if (loader is null && saver is null)
                throw new ArgumentNullException("loader/saver", "Cannot set both loader and saver to null.");

            var newDelegate = new LoadSaveDelegate(type, loader, saver);
            for (int i = 0; i < loadSaveDelegates.Count; i++)
            {
                var loadSaveDelegate = loadSaveDelegates[i];
                if (loadSaveDelegate.FileType == type)
                {
                    loadSaveDelegates[i] = newDelegate;
                    return;
                }
            }
            loadSaveDelegates.Add(newDelegate);
        }

        /// <summary>
        ///   Gets a pointer to the image buffer in memory.
        /// </summary>
        /// <value>A pointer to the image buffer in memory.</value>
        public IntPtr DataPointer => buffer;

        /// <summary>
        ///   Provides access to all pixel buffers of this image.
        /// </summary>
        /// <remarks>
        ///   For Texture3D, each Z slice of the Texture3D has a <see cref="PixelBufferArray"/> for each mipmap level.
        ///   For other textures, there is Description.MipLevels * Description.ArraySize pixel buffers.
        /// </remarks>
        public PixelBufferArray PixelBuffer => pixelBufferArray;

        /// <summary>
        ///   Gets the total number of bytes occupied by this image in memory.
        /// </summary>
        public int TotalSizeInBytes => totalSizeInBytes;

        /// <summary>
        ///   Gets the databox from this image.
        /// </summary>
        /// <returns>The databox of this image.</returns>
        public DataBox[] ToDataBox() => (DataBox[]) dataBoxArray.Clone();

        /// <summary>
        ///   Gets the databox from this image.
        /// </summary>
        /// <returns>The databox of this image.</returns>
        private DataBox[] ComputeDataBox()
        {
            dataBoxArray = new DataBox[Description.ArraySize * Description.MipLevels];
            int i = 0;
            for (int arrayIndex = 0; arrayIndex < Description.ArraySize; arrayIndex++)
            {
                for (int mipIndex = 0; mipIndex < Description.MipLevels; mipIndex++)
                {
                    // Get the first Z-slize (a DataBox for a Texture3D is pointing to the whole texture)
                    var pixelBuffer = GetPixelBufferUnsafe(arrayIndex, 0, mipIndex);

                    dataBoxArray[i].DataPointer = pixelBuffer.DataPointer;
                    dataBoxArray[i].RowPitch = pixelBuffer.RowStride;
                    dataBoxArray[i].SlicePitch = pixelBuffer.BufferStride;
                    i++;
                }
            }
            return dataBoxArray;
        }

        /// <summary>
        ///   Creates a new instance of <see cref="Image"/> from an image description.
        /// </summary>
        /// <param name="description">The image description.</param>
        /// <returns>A new image.</returns>
        public static Image New(ImageDescription description) => New(description, dataPointer: IntPtr.Zero);

        /// <summary>
        ///   Creates a new instance of a 1D <see cref="Image"/>.
        /// </summary>
        /// <param name="width">The width of the image, in pixels.</param>
        /// <param name="mipMapCount">The mip map count.</param>
        /// <param name="format">The pixel format of the image.</param>
        /// <param name="arraySize">Size of the array. Specify 1 for a single image.</param>
        /// <returns>A new image.</returns>
        public static Image New1D(int width, MipMapCount mipMapCount, PixelFormat format, int arraySize = 1)
        {
            return New1D(width, mipMapCount, format, arraySize, dataPointer: IntPtr.Zero);
        }

        /// <summary>
        ///   Creates a new instance of a 2D <see cref="Image"/>.
        /// </summary>
        /// <param name="width">The width of the image, in pixels.</param>
        /// <param name="height">The height of the image, in pixels.</param>
        /// <param name="mipMapCount">The mip map count.</param>
        /// <param name="format">The pixel format of the image.</param>
        /// <param name="arraySize">Size of the array. Specify 1 for a single image.</param>
        /// <param name="rowStride">
        ///   The stride of a row in the buffer, in bytes. Only valid when <paramref name="mipMapCount"/> is 1 and
        ///   <paramref name="format"/> is not a compressed pixel format.
        /// </param>
        /// <returns>A new image.</returns>
        public static Image New2D(int width, int height, MipMapCount mipMapCount, PixelFormat format, int arraySize = 1, int rowStride = 0)
        {
            return New2D(width, height, mipMapCount, format, arraySize, dataPointer: IntPtr.Zero, rowStride);
        }

        /// <summary>
        ///   Creates a new instance of a Cube <see cref="Image"/>.
        /// </summary>
        /// <param name="width">The width of the image, in pixels.</param>
        /// <param name="mipMapCount">The mip map count.</param>
        /// <param name="format">The pixel format of the image.</param>
        /// <returns>A new image.</returns>
        public static Image NewCube(int width, MipMapCount mipMapCount, PixelFormat format)
        {
            return NewCube(width, mipMapCount, format, dataPointer: IntPtr.Zero);
        }

        /// <summary>
        ///   Creates a new instance of a 3D <see cref="Image"/>.
        /// </summary>
        /// <param name="width">The width of the image, in pixels.</param>
        /// <param name="height">The height of the image, in pixels.</param>
        /// <param name="depth">The depth of the image, in pixels.</param>
        /// <param name="mipMapCount">The mip map count.</param>
        /// <param name="format">The pixel format of the image.</param>
        /// <returns>A new image.</returns>
        public static Image New3D(int width, int height, int depth, MipMapCount mipMapCount, PixelFormat format)
        {
            return New3D(width, height, depth, mipMapCount, format, dataPointer: IntPtr.Zero);
        }

        /// <summary>
        ///   Creates a new instance of <see cref="Image"/> from an image description.
        /// </summary>
        /// <param name="description">The image description.</param>
        /// <param name="dataPointer">Pointer to an existing buffer.</param>
        /// <returns>A new image.</returns>
        public static Image New(ImageDescription description, IntPtr dataPointer)
        {
            return new Image(description, dataPointer, offset: 0, handle: null, bufferIsDisposable: false);
        }

        /// <summary>
        ///   Initializes a new instance of <see cref="Image"/>.
        /// </summary>
        /// <param name="description">The image description.</param>
        /// <param name="dataPointer">The pointer to the data buffer.</param>
        /// <param name="offset">The offset from the beginning of the data buffer, in bytes.</param>
        /// <param name="handle">The handle of a pinned buffer (optionnal).</param>
        /// <param name="bufferIsDisposable">A value indicating if the buffer is disposable.</param>
        /// <exception cref="InvalidOperationException">If the format is invalid, or width/height/depth/arraysize is invalid with respect to the dimension.</exception>
        public static Image New(ImageDescription description, IntPtr dataPointer, int offset, GCHandle? handle, bool bufferIsDisposable)
        {
            return new Image(description, dataPointer, offset, handle, bufferIsDisposable);
        }

        /// <summary>
        ///   Creates a new instance of a 1D <see cref="Image"/>.
        /// </summary>
        /// <param name="width">The width of the image, in pixels.</param>
        /// <param name="mipMapCount">The mip map count.</param>
        /// <param name="format">The pixel format of the image.</param>
        /// <param name="arraySize">Size of the array.</param>
        /// <param name="dataPointer">Pointer to an existing buffer.</param>
        /// <returns>A new image.</returns>
        public static Image New1D(int width, MipMapCount mipMapCount, PixelFormat format, int arraySize, IntPtr dataPointer)
        {
            return new Image(
                CreateDescription(TextureDimension.Texture1D, width, height: 1, depth: 1, mipMapCount, format, arraySize),
                dataPointer, offset: 0, handle: null, bufferIsDisposable: false);
        }

        /// <summary>
        ///   Creates a new instance of a 2D <see cref="Image"/>.
        /// </summary>
        /// <param name="width">The width of the image, in pixels.</param>
        /// <param name="height">The height of the image, in pixels.</param>
        /// <param name="mipMapCount">The mip map count.</param>
        /// <param name="format">The pixel format of the image.</param>
        /// <param name="arraySize">Size of the array. Specify 1 for a single image.</param>
        /// <param name="dataPointer">Pointer to an existing buffer.</param>
        /// <param name="rowStride">
        ///   The stride of a row in the buffer, in bytes. Only valid when <paramref name="mipMapCount"/> is 1 and
        ///   <paramref name="format"/> is not a compressed pixel format.
        /// </param>
        /// <returns>A new image.</returns>
        public static Image New2D(int width, int height, MipMapCount mipMapCount, PixelFormat format, int arraySize, IntPtr dataPointer, int rowStride = 0)
        {
            return new Image(
                CreateDescription(TextureDimension.Texture2D, width, height, depth: 1, mipMapCount, format, arraySize),
                dataPointer, offset: 0, handle: null, bufferIsDisposable: false, PitchFlags.None, rowStride);
        }

        /// <summary>
        ///   Creates a new instance of a Cube <see cref="Image"/>.
        /// </summary>
        /// <param name="width">The width of the image, in pixels.</param>
        /// <param name="mipMapCount">The mip map count.</param>
        /// <param name="format">The pixel format of the image.</param>
        /// <param name="dataPointer">Pointer to an existing buffer.</param>
        /// <returns>A new image.</returns>
        public static Image NewCube(int width, MipMapCount mipMapCount, PixelFormat format, IntPtr dataPointer)
        {
            return new Image(
                CreateDescription(TextureDimension.TextureCube, width, width, depth: 1, mipMapCount, format, arraySize: 6),
                dataPointer, offset: 0, handle: null, bufferIsDisposable: false);
        }

        /// <summary>
        ///   Creates a new instance of a 3D <see cref="Image"/>.
        /// </summary>
        /// <param name="width">The width of the image, in pixels.</param>
        /// <param name="height">The height of the image, in pixels.</param>
        /// <param name="depth">The depth of the image, in pixels.</param>
        /// <param name="mipMapCount">The mip map count.</param>
        /// <param name="format">The pixel format of the image.</param>
        /// <param name="dataPointer">Pointer to an existing buffer.</param>
        /// <returns>A new image.</returns>
        public static Image New3D(int width, int height, int depth, MipMapCount mipMapCount, PixelFormat format, IntPtr dataPointer)
        {
            return new Image(
                CreateDescription(TextureDimension.Texture3D, width, height, depth, mipMapCount, format, arraySize: 1),
                dataPointer, offset: 0, handle: null, bufferIsDisposable: false);
        }

        /// <summary>
        ///   Loads an image from an unmanaged memory pointer.
        /// </summary>
        /// <param name="dataBuffer">
        ///   Pointer to an unmanaged memory buffer. If <see cref="makeACopy"/> is <c>false</c>, this buffer must be
        ///   allocated with <see cref="Utilities.AllocateMemory"/>.
        /// </param>
        /// <param name="makeACopy">A value indicating whether to copy the contents of the buffer to a new allocated buffer.</param>
        /// <param name="loadAsSRGB">A value indicating whether the image should be loaded as an sRGB texture.</param>
        /// <returns>An new image.</returns>
        /// <remarks>
        ///   If <paramref name="makeACopy"/> is set to <c>false</c>, the returned image is now the holder of the
        ///   unmanaged pointer and will release it on <see cref="Dispose"/>.
        /// </remarks>
        public static Image Load(DataPointer dataBuffer, bool makeACopy = false, bool loadAsSRGB = false)
        {
            return Load(dataBuffer.Pointer, dataBuffer.Size, makeACopy, loadAsSRGB);
        }

        /// <summary>
        ///   Loads an image from an unmanaged memory pointer.
        /// </summary>
        /// <param name="dataPointer">
        ///   Pointer to an unmanaged memory buffer. If <see cref="makeACopy"/> is <c>false</c>, this buffer must be
        ///   allocated with <see cref="Utilities.AllocateMemory"/>.
        /// </param>
        /// <param name="dataSize">Size of the unmanaged buffer.</param>
        /// <param name="makeACopy">A value indicating whether to copy the contents of the buffer to a new allocated buffer.</param>
        /// <param name="loadAsSRGB">A value indicating whether the image should be loaded as an sRGB texture.</param>
        /// <returns>An new image.</returns>
        /// <remarks>
        ///   If <paramref name="makeACopy"/> is set to <c>false</c>, the returned image is now the holder of the
        ///   unmanaged pointer and will release it on <see cref="Dispose"/>.
        /// </remarks>
        public static Image Load(IntPtr dataPointer, int dataSize, bool makeACopy = false, bool loadAsSRGB = false)
        {
            return Load(dataPointer, dataSize, makeACopy, null, loadAsSRGB);
        }

        /// <summary>
        ///   Loads an image from a managed buffer.
        /// </summary>
        /// <param name="buffer">Reference to a managed buffer.</param>
        /// <param name="loadAsSRGB">A value indicating whether the image should be loaded as an sRGB texture.</param>
        /// <returns>An new image.</returns>
        /// <remarks>
        ///   This method supports the following format: <c>DDS</c>, <c>BMP</c>, <c>JPG</c>, <c>PNG</c>, <c>GIF</c>, <c>TIFF</c>, <c>WMP</c>, <c>TGA</c>.
        /// </remarks>
        public static unsafe Image Load(byte[] buffer, bool loadAsSRGB = false)
        {
            if (buffer is null)
                throw new ArgumentNullException(nameof(buffer));

            int size = buffer.Length;

            // If buffer is allocated on Larget Object Heap, then we are going to pin it instead of making a copy.
            if (size > (85 * 1024))
            {
                var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                return Load(handle.AddrOfPinnedObject(), size, makeACopy: false, handle, loadAsSRGB);
            }

            fixed (void* pBuffer = buffer)
            {
                return Load((IntPtr) pBuffer, size, makeACopy: true, loadAsSRGB);
            }
        }

        /// <summary>
        ///   Loads an image from a stream.
        /// </summary>
        /// <param name="imageStream">The image stream.</param>
        /// <param name="loadAsSRGB">A value indicating whether the image should be loaded as an sRGB texture.</param>
        /// <returns>An new image.</returns>
        /// <remarks>
        ///   This method supports the following format: <c>DDS</c>, <c>BMP</c>, <c>JPG</c>, <c>PNG</c>, <c>GIF</c>, <c>TIFF</c>, <c>WMP</c>, <c>TGA</c>.
        /// </remarks>
        public static Image Load(Stream imageStream, bool loadAsSRGB = false)
        {
            if (imageStream is null)
                throw new ArgumentNullException(nameof(imageStream));

            // Read the whole stream into memory
            return Load(Utilities.ReadStream(imageStream), loadAsSRGB);
        }

        /// <summary>
        ///   Saves this image to a stream.
        /// </summary>
        /// <param name="imageStream">The destination stream.</param>
        /// <param name="fileType">Output file format.</param>
        /// <remarks>
        ///   This method supports the following format: <c>DDS</c>, <c>BMP</c>, <c>JPG</c>, <c>PNG</c>, <c>GIF</c>, <c>TIFF</c>, <c>WMP</c>, <c>TGA</c>.
        /// </remarks>
        public void Save(Stream imageStream, ImageFileType fileType)
        {
            if (imageStream is null)
                throw new ArgumentNullException(nameof(imageStream));

            Save(PixelBuffers, PixelBuffers.Length, Description, imageStream, fileType);
        }

        /// <summary>
        ///   Calculates the number of mipmap levels for a 1D Texture.
        /// </summary>
        /// <param name="width">The width of the texture, in pixels.</param>
        /// <param name="mipLevels">
        ///   A <see cref="MipMapCount"/>. Set to <c>true</c> to calculate all mipmaps. Set to <c>false</c> to calculate
        ///   only one miplevel. Set to any number greater than one to calculate a specific amount of levels.
        /// </param>
        /// <returns>The number of miplevels.</returns>
        public static int CalculateMipLevels(int width, MipMapCount mipLevels)
        {
            if (mipLevels > 1)
            {
                int maxMips = CountMips(width);
                if (mipLevels > maxMips)
                    throw new InvalidOperationException($"MipLevels must be <= {maxMips}.");
            }
            else if (mipLevels == 0)
            {
                mipLevels = CountMips(width);
            }
            else
            {
                mipLevels = 1;
            }
            return mipLevels;
        }

        /// <summary>
        ///   Calculates the number of mipmap levels for a 2D Texture.
        /// </summary>
        /// <param name="width">The width of the texture, in pixels.</param>
        /// <param name="height">The height of the texture, in pixels.</param>
        /// <param name="mipLevels">
        ///   A <see cref="MipMapCount"/>. Set to <c>true</c> to calculate all mipmaps. Set to <c>false</c> to calculate
        ///   only one miplevel. Set to any number greater than one to calculate a specific amount of levels.
        /// </param>
        /// <returns>The number of miplevels.</returns>
        public static int CalculateMipLevels(int width, int height, MipMapCount mipLevels)
        {
            if (mipLevels > 1)
            {
                int maxMips = CountMips(width, height);
                if (mipLevels > maxMips)
                    throw new InvalidOperationException($"MipLevels must be <= {maxMips}.");
            }
            else if (mipLevels == 0)
            {
                mipLevels = CountMips(width, height);
            }
            else
            {
                mipLevels = 1;
            }
            return mipLevels;
        }

        /// <summary>
        ///   Calculates the number of mipmap levels for a 3D Texture.
        /// </summary>
        /// <param name="width">The width of the texture, in pixels.</param>
        /// <param name="height">The height of the texture, in pixels.</param>
        /// <param name="depth">The depth of the texture, in pixels.</param>
        /// <param name="mipLevels">
        ///   A <see cref="MipMapCount"/>. Set to <c>true</c> to calculate all mipmaps. Set to <c>false</c> to calculate
        ///   only one miplevel. Set to any number greater than one to calculate a specific amount of levels.
        /// </param>
        /// <returns>The number of miplevels.</returns>
        public static int CalculateMipLevels(int width, int height, int depth, MipMapCount mipLevels)
        {
            if (mipLevels > 1)
            {
                if (!IsPow2(width) || !IsPow2(height) || !IsPow2(depth))
                    throw new InvalidOperationException("Width/Height/Depth must be power of 2.");

                int maxMips = CountMips(width, height, depth);
                if (mipLevels > maxMips)
                    throw new InvalidOperationException($"MipLevels must be <= {maxMips}.");
            }
            else if (mipLevels == 0)
            {
                if (!IsPow2(width) || !IsPow2(height) || !IsPow2(depth))
                    throw new InvalidOperationException("Width/Height/Depth must be power of 2.");

                mipLevels = CountMips(width, height, depth);
            }
            else
            {
                mipLevels = 1;
            }
            return mipLevels;
        }

        public static int CalculateMipSize(int width, int mipLevel)
        {
            mipLevel = Math.Min(mipLevel, CountMips(width));
            width >>= mipLevel;
            return width > 0 ? width : 1;
        }

        /// <summary>
        ///   Loads an image from the specified pointer.
        /// </summary>
        /// <param name="dataPointer">The data pointer.</param>
        /// <param name="dataSize">Size of the data.</param>
        /// <param name="makeACopy"><c>true</c> to make a copy of the buffer.</param>
        /// <param name="handle">The handle of a pinned buffer.</param>
        /// <param name="loadAsSRGB">Whether the image should be loaded as an sRGB texture.</param>
        /// <returns>An image.</returns>
        /// <exception cref="NotSupportedException">The image format is not supported.</exception>
        private static Image Load(IntPtr dataPointer, int dataSize, bool makeACopy, GCHandle? handle, bool loadAsSRGB = true)
        {
            foreach (var loadSaveDelegate in loadSaveDelegates)
            {
                if (loadSaveDelegate.Load != null)
                {
                    var image = loadSaveDelegate.Load(dataPointer, dataSize, makeACopy, handle);
                    if (image != null)
                    {
                        if (loadAsSRGB)
                            image.ConvertFormatToSRgb();

                        return image;
                    }
                }
            }
            throw new NotSupportedException("Image format not supported.");
        }

        /// <summary>
        ///   Saves this image to a stream.
        /// </summary>
        /// <param name="pixelBuffers">The buffers to save.</param>
        /// <param name="count">The number of buffers to save.</param>
        /// <param name="description">Global description of the buffer.</param>
        /// <param name="imageStream">The destination stream.</param>
        /// <param name="fileType">Specify the output format.</param>
        /// <remarks>
        ///   This method supports the following format: <c>DDS</c>, <c>BMP</c>, <c>JPG</c>, <c>PNG</c>, <c>GIF</c>, <c>TIFF</c>, <c>WMP</c>, <c>TGA</c>.
        /// </remarks>
        /// <exception cref="NotSupportedException">The image format is not supported.</exception>
        internal static void Save(PixelBuffer[] pixelBuffers, int count, ImageDescription description, Stream imageStream, ImageFileType fileType)
        {
            foreach (var loadSaveDelegate in loadSaveDelegates)
            {
                if (loadSaveDelegate.FileType == fileType)
                {
                    loadSaveDelegate.Save(pixelBuffers, count, description, imageStream);
                    return;
                }
            }
            throw new NotSupportedException("Image format not supported.");
        }

        static Image()
        {
            Register(ImageFileType.Stride, ImageHelper.LoadFromMemory, ImageHelper.SaveFromMemory);
            Register(ImageFileType.Dds, DDSHelper.LoadFromDDSMemory, DDSHelper.SaveToDDSStream);
            Register(ImageFileType.Gif, StandardImageHelper.LoadFromMemory, StandardImageHelper.SaveGifFromMemory);
            Register(ImageFileType.Tiff, StandardImageHelper.LoadFromMemory, StandardImageHelper.SaveTiffFromMemory);
            Register(ImageFileType.Bmp, StandardImageHelper.LoadFromMemory, StandardImageHelper.SaveBmpFromMemory);
            Register(ImageFileType.Jpg, StandardImageHelper.LoadFromMemory, StandardImageHelper.SaveJpgFromMemory);
            Register(ImageFileType.Png, StandardImageHelper.LoadFromMemory, StandardImageHelper.SavePngFromMemory);
            Register(ImageFileType.Wmp, StandardImageHelper.LoadFromMemory, StandardImageHelper.SaveWmpFromMemory);
        }

        internal unsafe void Initialize(ImageDescription description, IntPtr dataPointer, int offset, GCHandle? handle, bool bufferIsDisposable, PitchFlags pitchFlags = PitchFlags.None, int rowStride = 0)
        {
            if (!description.Format.IsValid() || description.Format.IsVideo())
                throw new InvalidOperationException("Unsupported DXGI Format.");

            if (rowStride > 0 && description.MipLevels != 1)
                throw new InvalidOperationException("Cannot specify custom stride with mipmaps.");

            this.handle = handle;

            switch (description.Dimension)
            {
                case TextureDimension.Texture1D:
                    if (description.Width <= 0 ||
                        description.Height != 1 ||
                        description.Depth != 1 ||
                        description.ArraySize == 0)
                        throw new InvalidOperationException("Invalid Width/Height/Depth/ArraySize for Image 1D.");

                    // Check that miplevels are fine
                    description.MipLevels = CalculateMipLevels(description.Width, description.MipLevels);
                    break;

                case TextureDimension.Texture2D:
                case TextureDimension.TextureCube:
                    if (description.Width <= 0 ||
                        description.Height <= 0 ||
                        description.Depth != 1 ||
                        description.ArraySize == 0)
                        throw new InvalidOperationException("Invalid Width/Height/Depth/ArraySize for Image 2D.");

                    if (description.Dimension == TextureDimension.TextureCube)
                    {
                        if ((description.ArraySize % 6) != 0)
                            throw new InvalidOperationException("TextureCube must have an array size of 6.");
                    }

                    // Check that miplevels are fine
                    description.MipLevels = CalculateMipLevels(description.Width, description.Height, description.MipLevels);
                    break;

                case TextureDimension.Texture3D:
                    if (description.Width <= 0 ||
                        description.Height <= 0 ||
                        description.Depth <= 0 ||
                        description.ArraySize != 1)
                        throw new InvalidOperationException("Invalid Width/Height/Depth/ArraySize for Image 3D.");

                    // Check that miplevels are fine
                    description.MipLevels = CalculateMipLevels(description.Width, description.Height, description.Depth, description.MipLevels);
                    break;
            }

            // Calculate mipmaps
            mipMapToZIndex = CalculateImageArray(description, pitchFlags, rowStride, out int pixelBufferCount, out totalSizeInBytes);
            mipmapDescriptions = CalculateMipMapDescription(description);
            zBufferCountPerArraySlice = mipMapToZIndex[mipMapToZIndex.Count - 1];

            // Allocate all pixel buffers
            PixelBuffers = new PixelBuffer[pixelBufferCount];
            pixelBufferArray = new PixelBufferArray(this);

            // Setup all pointers
            // (only release buffer that is not pinned and is asked to be disposed)
            this.bufferIsDisposable = !handle.HasValue && bufferIsDisposable;
            buffer = dataPointer;

            if (dataPointer == IntPtr.Zero)
            {
                buffer = Utilities.AllocateMemory(totalSizeInBytes);
                offset = 0;
                this.bufferIsDisposable = true;
            }

            SetupImageArray((IntPtr)((byte*)buffer + offset), rowStride, description, pitchFlags, PixelBuffers);

            Description = description;

            // PreCompute databoxes
            dataBoxArray = ComputeDataBox();
        }

        internal void InitializeFrom(Image image)
        {
            // TODO: Invalidate original image?
            PixelBuffers = image.PixelBuffers;
            dataBoxArray = image.dataBoxArray;
            mipMapToZIndex = image.mipMapToZIndex;
            zBufferCountPerArraySlice = image.zBufferCountPerArraySlice;
            mipmapDescriptions = image.mipmapDescriptions;
            pixelBufferArray = image.pixelBufferArray;
            totalSizeInBytes = image.totalSizeInBytes;
            buffer = image.buffer;
            bufferIsDisposable = image.bufferIsDisposable;
            handle = image.handle;
            Description = image.Description;
        }

        private PixelBuffer GetPixelBufferUnsafe(int arrayIndex, int zIndex, int mipmap)
        {
            var depthIndex = mipMapToZIndex[mipmap];
            var pixelBufferIndex = arrayIndex * zBufferCountPerArraySlice + depthIndex + zIndex;

            return PixelBuffers[pixelBufferIndex];
        }

        private static ImageDescription CreateDescription(TextureDimension dimension, int width, int height, int depth, MipMapCount mipMapCount, PixelFormat format, int arraySize)
        {
            return new ImageDescription()
                       {
                           Width = width,
                           Height = height,
                           Depth = depth,
                           ArraySize = arraySize,
                           Dimension = dimension,
                           Format = format,
                           MipLevels = mipMapCount,
                       };
        }

        [Flags]
        internal enum PitchFlags
        {
            /// <summary>
            ///   Normal operation.
            /// </summary>
            None = 0x0,

            /// <summary>
            ///   Assume pitch is DWORD aligned instead of BYTE aligned.
            /// </summary>
            LegacyDword = 0x1,

            /// <summary>
            ///   Override with a legacy 24 bits-per-pixel format size.
            /// </summary>
            Bpp24 = 0x10000,

            /// <summary>
            ///   Override with a legacy 16 bits-per-pixel format size.
            /// </summary>
            Bpp16 = 0x20000,

            /// <summary>
            ///   Override with a legacy 8 bits-per-pixel format size.
            /// </summary>
            Bpp8 = 0x40000
        }

        internal static void ComputePitch(PixelFormat format, int width, int height, out int rowPitch, out int slicePitch, out int widthCount, out int heightCount, PitchFlags flags = PitchFlags.None)
        {
            widthCount = width;
            heightCount = height;

            if (format.IsCompressed())
            {
                int minWidth = 1;
                int minHeight = 1;
                int bpb;

                switch (format)
                {
                    case PixelFormat.BC1_Typeless:
                    case PixelFormat.BC1_UNorm:
                    case PixelFormat.BC1_UNorm_SRgb:
                    case PixelFormat.BC4_Typeless:
                    case PixelFormat.BC4_UNorm:
                    case PixelFormat.BC4_SNorm:
                    case PixelFormat.ETC1:
                        bpb = 8;
                        break;

                    default:
                        bpb = 16;
                        break;
                }

                widthCount = Math.Max(1, (Math.Max(minWidth, width) + 3)) / 4;
                heightCount = Math.Max(1, (Math.Max(minHeight, height) + 3)) / 4;
                rowPitch = widthCount * bpb;

                slicePitch = rowPitch * heightCount;
            }
            else if (format.IsPacked())
            {
                rowPitch = ((width + 1) >> 1) * 4;

                slicePitch = rowPitch * height;
            }
            else
            {
                int bitsPerPixel;

                if (flags.HasFlag(PitchFlags.Bpp24))
                    bitsPerPixel = 24;
                else if (flags.HasFlag(PitchFlags.Bpp16))
                    bitsPerPixel = 16;
                else if (flags.HasFlag(PitchFlags.Bpp8))
                    bitsPerPixel = 8;
                else
                    bitsPerPixel = format.SizeInBits();

                if (flags.HasFlag(PitchFlags.LegacyDword))
                {
                    // Special computation for some incorrectly created DDS files based on
                    // legacy DirectDraw assumptions about pitch alignment
                    rowPitch = ((width * bitsPerPixel + 31) / 32) * sizeof(int);
                    slicePitch = rowPitch * height;
                }
                else
                {
                    rowPitch = (width * bitsPerPixel + 7) / 8;
                    slicePitch = rowPitch * height;
                }
            }
        }

        internal static MipMapDescription[] CalculateMipMapDescription(ImageDescription metadata)
        {
            return CalculateMipMapDescription(metadata, out _, out _);
        }

        internal static MipMapDescription[] CalculateMipMapDescription(ImageDescription metadata, out int nImages, out int pixelSize)
        {
            pixelSize = 0;
            nImages = 0;

            int width = metadata.Width;
            int height = metadata.Height;
            int depth = metadata.Depth;

            var mipmaps = new MipMapDescription[metadata.MipLevels];

            for (int level = 0; level < metadata.MipLevels; ++level)
            {
                ComputePitch(
                    metadata.Format,
                    width,
                    height,
                    out int rowPitch,
                    out int slicePitch,
                    out int widthPacked,
                    out int heightPacked,
                    PitchFlags.None);

                mipmaps[level] = new MipMapDescription(
                    width,
                    height,
                    depth,
                    rowPitch,
                    slicePitch,
                    widthPacked,
                    heightPacked);

                pixelSize += depth * slicePitch;
                nImages += depth;

                if (height > 1)
                    height >>= 1;

                if (width > 1)
                    width >>= 1;

                if (depth > 1)
                    depth >>= 1;
            }
            return mipmaps;
        }

        /// <summary>
        ///   Determines the number of image array entries and the pixel size.
        /// </summary>
        /// <param name="imageDesc">Description of the image to create.</param>
        /// <param name="pitchFlags">Pitch flags.</param>
        /// <param name="bufferCount">Output number of mipmap.</param>
        /// <param name="pixelSizeInBytes">Output total size to allocate pixel buffers for all images.</param>
        private static List<int> CalculateImageArray(ImageDescription imageDesc, PitchFlags pitchFlags, int rowStride, out int bufferCount, out int pixelSizeInBytes)
        {
            pixelSizeInBytes = 0;
            bufferCount = 0;

            var mipmapToZIndex = new List<int>();

            for (int arrayIndex = 0; arrayIndex < imageDesc.ArraySize; arrayIndex++)
            {
                int width = imageDesc.Width;
                int height = imageDesc.Height;
                int depth = imageDesc.Depth;

                for (int i = 0; i < imageDesc.MipLevels; i++)
                {
                    ComputePitch(
                        imageDesc.Format,
                        width,
                        height,
                        out int rowPitch,
                        out int slicePitch,
                        out int widthPacked,
                        out int heightPacked,
                        pitchFlags);

                    if (rowStride > 0)
                    {
                        // Check that stride is correct
                        if (rowStride < rowPitch)
                            throw new InvalidOperationException($"Invalid stride [{rowStride}]. Value can't be lower than actual stride [{rowPitch}].");

                        if (widthPacked != width || heightPacked != height)
                            throw new InvalidOperationException("Custom strides are not supported with packed PixelFormats.");

                        // Override row pitch
                        rowPitch = rowStride;

                        // Recalculate slice pitch
                        slicePitch = rowStride * height;
                    }

                    // Store the number of Z-slices per miplevel
                    if (arrayIndex == 0)
                        mipmapToZIndex.Add(bufferCount);

                    // Keep a trace of indices for the 1st array size, for each mip level
                    pixelSizeInBytes += depth * slicePitch;
                    bufferCount += depth;

                    if (height > 1)
                        height >>= 1;

                    if (width > 1)
                        width >>= 1;

                    if (depth > 1)
                        depth >>= 1;
                }

                // For the last mipmaps, store just the number of Z-slices in total
                if (arrayIndex == 0)
                    mipmapToZIndex.Add(bufferCount);
            }
            return mipmapToZIndex;
        }

        /// <summary>
        ///   Allocates the pixel buffers for the image.
        /// </summary>
        /// <param name="buffer">Pointer to the image data buffer.</param>
        /// <param name="imageDesc">Description of the image.</param>
        /// <param name="pitchFlags">One or more values of <see cref="PitchFlags"/> indicating how to process the pitch of the image.</param>
        /// <param name="output">Array of pixel buffers to initialize with the data.</param>
        private static unsafe void SetupImageArray(IntPtr buffer, int rowStride, ImageDescription imageDesc, PitchFlags pitchFlags, PixelBuffer[] output)
        {
            int index = 0;
            var pixels = (byte*) buffer;
            for (uint arrayIndex = 0; arrayIndex < imageDesc.ArraySize; ++arrayIndex)
            {
                int width = imageDesc.Width;
                int height = imageDesc.Height;
                int depth = imageDesc.Depth;

                for (uint mipLevel = 0; mipLevel < imageDesc.MipLevels; ++mipLevel)
                {
                    ComputePitch(
                        imageDesc.Format,
                        width,
                        height,
                        out int rowPitch,
                        out int slicePitch,
                        out int widthPacked,
                        out int heightPacked,
                        pitchFlags);

                    if (rowStride > 0)
                    {
                        // Check that stride is correct
                        if (rowStride < rowPitch)
                            throw new InvalidOperationException($"Invalid stride [{rowStride}]. Value can't be lower than actual stride [{rowPitch}].");

                        if (widthPacked != width || heightPacked != height)
                            throw new InvalidOperationException("Custom strides are not supported with packed PixelFormats.");

                        // Override row pitch
                        rowPitch = rowStride;

                        // Recalculate slice pitch
                        slicePitch = rowStride * height;
                    }

                    for (uint zSlice = 0; zSlice < depth; ++zSlice)
                    {
                        // We use the same memory organization that Direct3D 11 needs for D3D11_SUBRESOURCE_DATA
                        // with all slices of a given miplevel being continuous in memory
                        output[index] = new PixelBuffer(width, height, imageDesc.Format, rowPitch, slicePitch, (IntPtr) pixels);
                        ++index;

                        pixels += slicePitch;
                    }

                    if (height > 1)
                        height >>= 1;

                    if (width > 1)
                        width >>= 1;

                    if (depth > 1)
                        depth >>= 1;
                }
            }
        }

        private static bool IsPow2(int x)
        {
            return ((x != 0) && (x & (x - 1)) == 0);
        }

        public static int CountMips(int width)
        {
            int mipLevels = 1;

            while (width > 1)
            {
                ++mipLevels;

                width >>= 1;
            }

            return mipLevels;
        }

        public static int CountMips(int width, int height)
        {
            int mipLevels = 1;

            while (height > 1 || width > 1)
            {
                ++mipLevels;

                if (height > 1)
                    height >>= 1;

                if (width > 1)
                    width >>= 1;
            }

            return mipLevels;
        }

        public static int CountMips(int width, int height, int depth)
        {
            int mipLevels = 1;

            while (height > 1 || width > 1 || depth > 1)
            {
                ++mipLevels;

                if (height > 1)
                    height >>= 1;

                if (width > 1)
                    width >>= 1;

                if (depth > 1)
                    depth >>= 1;
            }

            return mipLevels;
        }

        private class LoadSaveDelegate
        {
            public LoadSaveDelegate(ImageFileType fileType, ImageLoadDelegate load, ImageSaveDelegate save)
            {
                FileType = fileType;
                Load = load;
                Save = save;
            }

            public ImageFileType FileType;

            public ImageLoadDelegate Load;

            public ImageSaveDelegate Save;
        }
   }
}
