// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;

using Stride.Core.Annotations;
using Stride.Core.Extensions;
using Stride.Core.Presentation.Commands;
using Stride.Core.Presentation.ViewModel;

namespace Stride.LauncherApp.ViewModels
{
    /// <summary>
    ///   Represents the release notes of a given version of Stride.
    /// </summary>
    internal class ReleaseNotesViewModel : DispatcherViewModel
    {
        private static readonly HttpClient httpClient = new();

        private readonly LauncherViewModel launcher;
        private bool isActive;
        private string markdownContent;
        private bool isLoading = true;
        private bool isLoaded;
        private bool isUnavailable;

        private const string RootUrl = "https://doc.stride3d.net";
        private const string ReleaseNotesFileName = "ReleaseNotes.md";
        private string baseUrl;

        public string BaseUrl { get { return baseUrl; } private set { SetValue(ref baseUrl, value); } }

        public string Version { get; }

        public string MarkdownContent { get { return markdownContent; } private set { SetValue(ref markdownContent, value); } }

        public bool IsActive { get { return isActive; } private set { SetValue(ref isActive, value); } }

        public bool IsLoading { get { return isLoading; } set { SetValue(ref isLoading, value); } }

        public bool IsLoaded { get { return isLoaded; } set { SetValue(ref isLoaded, value); } }

        public bool IsUnavailable { get { return isUnavailable; } set { SetValue(ref isUnavailable, value); } }

        public ICommandBase ToggleCommand { get; private set; }


        internal ReleaseNotesViewModel([NotNull] LauncherViewModel launcher, [NotNull] string version)
            : base(launcher.SafeArgument(nameof(launcher)).ServiceProvider)
        {
            if (version is null)
                throw new ArgumentNullException(nameof(version));

            this.launcher = launcher;

            Version = version;
            baseUrl = $"{RootUrl}/{Version}/ReleaseNotes/";
#if DEBUG
            if (Environment.CommandLine.ToLowerInvariant().Contains("/previewreleasenotes"))
            {
                var launcherPath = AppDomain.CurrentDomain.BaseDirectory;
                var mdPath = Path.Combine(launcherPath, @"..\..\..\..\..\doc\");
                if (File.Exists($"{mdPath}{ReleaseNotesFileName}"))
                {
                    baseUrl = $"file:///{mdPath.Replace("\\", "/")}";
                }
            }
#endif

            ToggleCommand = new AnonymousCommand(ServiceProvider, Toggle);
        }

        public string BaseUrl { get { return baseUrl; } }

        public string Version { get; }

        public string MarkdownContent { get { return markdownContent; } private set { SetValue(ref markdownContent, value); } }

        public bool IsActive { get { return isActive; } private set { SetValue(ref isActive, value); } }

        public bool IsLoading { get { return isLoading; } set { SetValue(ref isLoading, value); } }

        public bool IsLoaded { get { return isLoaded; } set { SetValue(ref isLoaded, value); } }

        public bool IsUnavailable { get { return isUnavailable; } set { SetValue(ref isUnavailable, value); } }

        public ICommandBase ToggleCommand { get; private set; }

        public async void FetchReleaseNotes()
        {
            string releaseNotesMarkdown;

            try
            {
                using (var response = await httpClient.GetAsync($"{BaseUrl}{ReleaseNotesFileName}"))
                {
                    response.EnsureSuccessStatusCode();
                    releaseNotesMarkdown = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception)
            {
                IsLoading = false;
                IsUnavailable = true;
                return;
            }

            if (releaseNotesMarkdown != null)
            {
                // Parse video tag
                var videoRegex = new Regex(@"
                    <video
                        [^>]*?                 # Any valid HTML characters
                        poster                 # Poster attribute
                        \s*=\s*
                        (['""])                # Quote char = $1
                        ([^'"" >]+?)           # URL of poster image
                        \1                     # Matching quote
                        [^>]*?>\s*
                            <source
                                [^>]*?         # Any valid HTML characters
                                src            # Src attribute
                                \s*=\s*
                                (['""])        # Quote char = $3
                                ([^'"" >] +?)  # URL of video
                                \3             # Matching quote
                                [^>] *?>\s*
                    </video>",
                    RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

                MarkdownContent = videoRegex.Replace(releaseNotesMarkdown, "![]($2)\r\n\r\n[_Click to watch the video_]($4)");
                IsLoading = false;
                IsLoaded = true;
            }
            else
            {
                IsLoading = false;
                IsUnavailable = true;
            }
        }

        public void Show()
        {
            IsActive = true;
            launcher.ActiveReleaseNotes = this;
        }

        private void Toggle()
        {
            IsActive = launcher.ActiveReleaseNotes != this || !IsActive;
            launcher.ActiveReleaseNotes = this;
        }
    }
}
