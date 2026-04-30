// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using Xunit;

using Stride.Core.IO;

namespace Stride.Core.Tests.IO
{
    public class DatabaseFileProviderTests
    {
        [Theory]
        [InlineData("Root/A", "Root", "A", VirtualSearchOption.TopDirectoryOnly)]
        [InlineData("Root/A", "Root", "A", VirtualSearchOption.AllDirectories)]
        [InlineData("Root/A", "Root/", "A", VirtualSearchOption.AllDirectories)]
        [InlineData("Root/A", "Root", "*", VirtualSearchOption.TopDirectoryOnly)]
        [InlineData("Root/A", "Root", "*", VirtualSearchOption.AllDirectories)]
        [InlineData("Root/A", "Root/", "*", VirtualSearchOption.AllDirectories)]
        [InlineData("Root/Dir/A", "Root", "A", VirtualSearchOption.AllDirectories)]
        [InlineData("Root/Dir/A", "Root", "*", VirtualSearchOption.AllDirectories)]
        [InlineData("Root/A", "Root", "?", VirtualSearchOption.AllDirectories)]
        [InlineData("Root/Abc", "Root", "A?c", VirtualSearchOption.AllDirectories)]
        [InlineData("Root/Abbc", "Root", "A*c", VirtualSearchOption.AllDirectories)]
        [InlineData("Root/Abbc", "Root", "A*", VirtualSearchOption.AllDirectories)]
        public void ListFilesRegex_Matches(string url, string pathPrefix, string fileNamePattern, VirtualSearchOption options)
        {
            var regex = DatabaseFileProvider.CreateRegexForFileSearch(pathPrefix, fileNamePattern, options);
            Assert.Matches(regex, url);
        }

        [Theory]
        [InlineData("Root/A", "Root", "B", VirtualSearchOption.TopDirectoryOnly)]
        [InlineData("Root/A", "Root", "B", VirtualSearchOption.AllDirectories)]
        [InlineData("Root/Dir/A", "Root", "B", VirtualSearchOption.AllDirectories)]
        [InlineData("Root/Dir/A", "Root", "*", VirtualSearchOption.TopDirectoryOnly)]
        [InlineData("Root/Abbc", "Root", "A?c", VirtualSearchOption.AllDirectories)]
        // if path starts with / it won't match because URLs in DatabaseFileProvider don't have a preceding /
        [InlineData("Root/A", "/Root", "A", VirtualSearchOption.AllDirectories)]
        public void ListFilesRegex_DoesNotMatch(string url, string pathPrefix, string fileNamePattern, VirtualSearchOption options)
        {
            var regex = DatabaseFileProvider.CreateRegexForFileSearch(pathPrefix, fileNamePattern, options);
            Assert.DoesNotMatch(regex, url);
        }
    }
}
