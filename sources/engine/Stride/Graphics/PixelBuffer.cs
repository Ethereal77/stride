// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.IO;

using Stride.Core;

namespace Stride.Graphics
{
    /// <summary>
    ///   Represents a buffer of pixels.
    /// </summary>
    public sealed class PixelBuffer
    {
        private readonly int width;
        private readonly int height;
        private PixelFormat format;

        private readonly int rowStride;
        private readonly int bufferStride;

        private readonly IntPtr dataPointer;

        private readonly int pixelSize;

        /// <summary>
        /// True when RowStride == sizeof(pixelformat) * width
        /// </summary>
        private readonly bool isStrictRowStride;


        /// <summary>
        ///   Initializes a new instance of the <see cref="PixelBuffer" /> class.
        /// </summary>
        /// <param name="width">The width of the pixel buffer, in pixels.</param>
        /// <param name="height">The height of the pixel buffer, in pixels.</param>
        /// <param name="format">The pixel format.</param>
        /// <param name="rowStride">The row pitch, in bytes.</param>
        /// <param name="bufferStride">The slice pitch, in bytes.</param>
        /// <param name="dataPointer">The pointer to the pixels data.</param>
        public PixelBuffer(int width, int height, PixelFormat format, int rowStride, int bufferStride, IntPtr dataPointer)
        {
            if (dataPointer == IntPtr.Zero)
                throw new ArgumentException("Pointer cannot be IntPtr.Zero.", nameof(dataPointer));

            this.width = width;
            this.height = height;
            this.format = format;
            this.rowStride = rowStride;
            this.bufferStride = bufferStride;
            this.dataPointer = dataPointer;
            this.pixelSize = format.SizeInBytes();
            this.isStrictRowStride = (pixelSize * width) == rowStride;
        }

        /// <summary>
        ///   Gets the width, in bytes.
        /// </summary>
        /// <value>The width, in bytes.</value>
        public int Width => width;

        /// <summary>
        ///   Gets the height, in bytes.
        /// </summary>
        /// <value>The height, in bytes.</value>
        public int Height => height;

        /// <summary>
        ///   Gets the pixel format.
        /// </summary>
        /// <value>The pixel format.</value>
        public PixelFormat Format => format;

        /// <summary>
        ///   Converts the format to sRGB.
        /// </summary>
        public void ConvertFormatToSRgb() => format = format.ToSRgb();

        /// <summary>
        ///   Converts the format to non-sRGB.
        /// </summary>
        public void ConvertFormatToNonSRgb() => format = format.ToNonSRgb();

        /// <summary>
        ///   Gets the pixel size, in bytes.
        /// </summary>
        /// <value>The pixel size, in bytes.</value>
        public int PixelSize => pixelSize;

        /// <summary>
        ///   Gets the row stride, in bytes.
        /// </summary>
        /// <value>The row stride, in bytes.</value>
        public int RowStride => rowStride;

        /// <summary>
        ///   Gets the total size of this pixel buffer, in bytes.
        /// </summary>
        /// <value>The size of the pixel buffer, in bytes.</value>
        public int BufferStride => bufferStride;

        /// <summary>
        ///   Gets the pointer to the pixel buffer data.
        /// </summary>
        /// <value>The pointer to the pixel buffer data.</value>
        public IntPtr DataPointer => dataPointer;


        /// <summary>
        ///   Copies this pixel buffer to a destination pixel buffer.
        /// </summary>
        /// <param name="pixelBuffer">The destination pixel buffer.</param>
        /// <remarks>
        ///   The destination pixel buffer must have exactly the same dimensions (width, height) and format
        ///   as this instance.
        ///   Destination buffer can have different row stride.
        /// </remarks>
        public unsafe void CopyTo(PixelBuffer pixelBuffer)
        {
            // Check that buffers are identical
            if (Width != pixelBuffer.Width ||
                Height != pixelBuffer.Height ||
                Format != pixelBuffer.Format)
            {
                throw new ArgumentException("Invalid destination pixelBuffer. Mush have same Width, Height and Format.", nameof(pixelBuffer));
            }

            // If buffers have same size, than we can copy it directly
            if (BufferStride == pixelBuffer.BufferStride)
            {
                Utilities.CopyMemory(pixelBuffer.DataPointer, DataPointer, BufferStride);
            }
            else
            {
                var srcPointer = (byte*) DataPointer;
                var dstPointer = (byte*) pixelBuffer.DataPointer;
                var rowStride = Math.Min(RowStride, pixelBuffer.RowStride);

                // Copy per scanline
                for (int i = 0; i < Height; i++)
                {
                    Utilities.CopyMemory(new IntPtr(dstPointer), new IntPtr(srcPointer), rowStride);
                    srcPointer += RowStride;
                    dstPointer += pixelBuffer.RowStride;
                }
            }
        }

        /// <summary>
        ///   Saves this pixel buffer to a stream.
        /// </summary>
        /// <param name="imageStream">The destination stream.</param>
        /// <param name="fileType">Specify the output format.</param>
        /// <remarks>
        ///   This method supports the following format: <c>DDS</c>, <c>BMP</c>, <c>JPG</c>, <c>PNG</c>, <c>GIF</c>, <c>TIFF</c>, <c>WMP</c>, <c>TGA</c>.
        /// </remarks>
        public void Save(Stream imageStream, ImageFileType fileType)
        {
            var description = new ImageDescription()
                {
                    Width = width,
                    Height = height,
                    Depth = 1,
                    ArraySize = 1,
                    Dimension = TextureDimension.Texture2D,
                    Format = format,
                    MipLevels = 1,
                };

            Image.Save(new[] { this }, 1, description, imageStream, fileType);
        }

        /// <summary>
        ///   Gets the pixel value at a specified position.
        /// </summary>
        /// <typeparam name="T">Type of the pixel data.</typeparam>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <returns>The pixel value.</returns>
        /// <remarks>
        ///   This method doesn't check bounding.
        /// </remarks>
        public unsafe T GetPixel<T>(int x, int y) where T : struct
        {
            return Utilities.Read<T>(new IntPtr(((byte*) DataPointer + RowStride * y + x * PixelSize)));
        }

        /// <summary>
        ///   Sets the pixel value at a specified position.
        /// </summary>
        /// <typeparam name="T">Type of the pixel data.</typeparam>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="value">The pixel value.</param>
        /// <remarks>
        ///   This method doesn't check bounding.
        /// </remarks>
        public unsafe void SetPixel<T>(int x, int y, T value) where T : struct
        {
            Utilities.Write(new IntPtr((byte*) DataPointer + RowStride * y + x * PixelSize), ref value);
        }

        /// <summary>
        ///   Gets the pixel values of a scanline from the buffer.
        /// </summary>
        /// <typeparam name="T">Type of the pixel data.</typeparam>
        /// <param name="yOffset">The Y line offset.</param>
        /// <returns>Scanline pixels from the buffer.</returns>
        /// <exception cref="ArgumentException">If the <c>sizeof(<typeparamref name="T"/>)</c> is an invalid size.</exception>
        /// <remarks>
        ///   This method works on a row basis. The <see cref="yOffset"/> specifies the first row to get the pixels from.
        /// </remarks>
        public T[] GetPixels<T>(int yOffset = 0) where T : struct
        {
            var sizeOfOutputPixel = Utilities.SizeOf<T>();
            var totalSize = Width * Height * pixelSize;
            if ((totalSize % sizeOfOutputPixel) != 0)
                throw new ArgumentException($"Invalid sizeof({nameof(T)}). Not a multiple of current size [{totalSize}].");

            var buffer = new T[totalSize / sizeOfOutputPixel];
            GetPixels(buffer, yOffset);
            return buffer;
        }

        /// <summary>
        ///   Gets the pixel values of a scanline from the buffer.
        /// </summary>
        /// <typeparam name="T">Type of the pixel data.</typeparam>
        /// <param name="pixels">Buffer where to copy the scanline pixel data.</param>
        /// <param name="yOffset">The Y line offset.</param>
        /// <returns>Scanline pixels from the buffer.</returns>
        /// <exception cref="ArgumentException">If the <c>sizeof(<typeparamref name="T"/>)</c> is an invalid size.</exception>
        /// <remarks>
        ///   This method works on a row basis. The <see cref="yOffset"/> specifies the first row to get the pixels from.
        /// </remarks>
        public void GetPixels<T>(T[] pixels, int yOffset = 0) where T : struct
        {
            GetPixels(pixels, yOffset, pixelIndex: 0, pixels.Length);
        }

        /// <summary>
        ///   Gets the pixel values of a scanline from the buffer.
        /// </summary>
        /// <typeparam name="T">Type of the pixel data.</typeparam>
        /// <param name="pixels">Buffer where to copy the scanline pixel data.</param>
        /// <param name="yOffset">The Y line offset.</param>
        /// <param name="pixelIndex">Offset into the destination <paramref name="pixels"/> buffer.</param>
        /// <param name="pixelCount">Number of pixels to write into the destination <paramref name="pixels"/> buffer.</param>
        /// <exception cref="ArgumentException">If the <c>sizeof(<typeparamref name="T"/>)</c> is an invalid size.</exception>
        /// <remarks>
        ///   This method works on a row basis. The <see cref="yOffset"/> specifies the first row to get the pixels from.
        /// </remarks>
        public unsafe void GetPixels<T>(T[] pixels, int yOffset, int pixelIndex, int pixelCount) where T : struct
        {
            var pixelPointer = (byte*) DataPointer + yOffset * rowStride;
            if (isStrictRowStride)
            {
                Utilities.Read(new IntPtr(pixelPointer), pixels, 0, pixelCount);
            }
            else
            {
                var sizeOfOutputPixel = Utilities.SizeOf<T>() * pixelCount;
                var sizePerWidth = sizeOfOutputPixel / Width;
                var remainingPixels = sizeOfOutputPixel % Width;
                for (int i = 0; i < sizePerWidth; i++)
                {
                    Utilities.Read(new IntPtr(pixelPointer), pixels, pixelIndex, Width);
                    pixelPointer += rowStride;
                    pixelIndex += Width;
                }
                if (remainingPixels > 0)
                {
                    Utilities.Read(new IntPtr(pixelPointer), pixels, pixelIndex, remainingPixels);
                }
            }
        }

        /// <summary>
        ///   Sets the pixel values of a scanline in the buffer.
        /// </summary>
        /// <typeparam name="T">Type of the pixel data.</typeparam>
        /// <param name="sourcePixels">Source pixel buffer.</param>
        /// <param name="yOffset">The Y line offset.</param>
        /// <exception cref="ArgumentException">If the <c>sizeof(<typeparamref name="T"/>)</c> is an invalid size.</exception>
        /// <remarks>
        ///   This method works on a row basis. The <see cref="yOffset"/> specifies the first row to get the pixels from.
        /// </remarks>
        public void SetPixels<T>(T[] sourcePixels, int yOffset = 0) where T : struct
        {
            SetPixels(sourcePixels, yOffset, pixelIndex: 0, sourcePixels.Length);
        }

        /// <summary>
        ///   Sets the pixel values of a scanline in the buffer.
        /// </summary>
        /// <typeparam name="T">Type of the pixel data.</typeparam>
        /// <param name="sourcePixels">Source pixel buffer</param>
        /// <param name="yOffset">The Y line offset.</param>
        /// <param name="pixelIndex">Offset into the source <paramref name="sourcePixels"/> buffer.</param>
        /// <param name="pixelCount">Number of pixels to write into the source <paramref name="sourcePixels"/> buffer.</param>
        /// <exception cref="ArgumentException">If the <c>sizeof(<typeparamref name="T"/>)</c> is an invalid size.</exception>
        /// <remarks>
        ///   This method works on a row basis. The <see cref="yOffset"/> specifies the first row to get the pixels from.
        /// </remarks>
        public unsafe void SetPixels<T>(T[] sourcePixels, int yOffset, int pixelIndex, int pixelCount) where T : struct
        {
            var pixelPointer = (byte*) DataPointer + yOffset * rowStride;
            if (isStrictRowStride)
            {
                Utilities.Write(new IntPtr(pixelPointer), sourcePixels, 0, pixelCount);
            }
            else
            {
                var sizeOfOutputPixel = Utilities.SizeOf<T>() * pixelCount;
                var sizePerWidth = sizeOfOutputPixel / Width;
                var remainingPixels = sizeOfOutputPixel % Width;
                for (int i = 0; i < sizePerWidth; i++)
                {
                    Utilities.Write(new IntPtr(pixelPointer), sourcePixels, pixelIndex, Width);
                    pixelPointer += rowStride;
                    pixelIndex += Width;
                }
                if (remainingPixels > 0)
                {
                    Utilities.Write(new IntPtr(pixelPointer), sourcePixels, pixelIndex, remainingPixels);
                }
            }
        }
    }
}
