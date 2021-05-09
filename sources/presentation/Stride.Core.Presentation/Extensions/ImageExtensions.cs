// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Stride.Core.Annotations;

namespace Stride.Core.Presentation.Extensions
{
    public static class ImageExtensions
    {
        public static void SetSource([NotNull] this Image image, [NotNull] Uri uri)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            image.Source = ImageSourceFromFile(uri);
        }

        public static void SetSource([NotNull] this Image image, [NotNull] string uri)
        {
            if (string.IsNullOrWhiteSpace(uri))
                throw new ArgumentException("Invalid 'uri' argument.");

            SetSource(image, new Uri(uri));
        }

        [NotNull]
        public static ImageSource ImageSourceFromFile([NotNull] Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var source = new BitmapImage();
            source.BeginInit();
            source.UriSource = uri;
            source.CacheOption = BitmapCacheOption.OnLoad;
            source.EndInit();

            return source;
        }

        [NotNull]
        public static ImageSource ImageSourceFromFile([NotNull] string uri)
        {
            if (string.IsNullOrWhiteSpace(uri))
                throw new ArgumentException("Invalid 'uri' argument.");

            return ImageSourceFromFile(new Uri(uri));
        }
    }
}
