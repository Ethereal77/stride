// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2009 SLNTools - Christian Warren
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using Stride.Core.Annotations;

namespace Stride.Core.VisualStudio
{
    /// <summary>
    /// A VisualStudio solution.
    /// </summary>
    [DebuggerDisplay("Projects = [{Projects.Count}]")]
    public class Solution
    {
        private readonly SectionCollection globalSections;
        private readonly List<string> headers;
        private readonly ProjectCollection projects;

        /// <summary>
        /// Initializes a new instance of the <see cref="Solution"/> class.
        /// </summary>
        public Solution()
        {
            FullPath = null;
            headers = new List<string>();
            projects = new ProjectCollection(this);
            globalSections = new SectionCollection();
            Properties = new PropertyItemCollection();
        }

        private Solution([NotNull] Solution original)
            : this(original.FullPath, original.Headers, original.Projects, original.GlobalSections, original.Properties)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Solution" /> class.
        /// </summary>
        /// <param name="fullpath">The fullpath.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="projects">The projects.</param>
        /// <param name="globalSections">The global sections.</param>
        /// <param name="properties">The properties.</param>
        public Solution(string fullpath, [NotNull] IEnumerable<string> headers, IEnumerable<Project> projects, IEnumerable<Section> globalSections, IEnumerable<PropertyItem> properties)
        {
            FullPath = fullpath;
            this.headers = new List<string>(headers);
            this.projects = new ProjectCollection(this, projects);
            this.globalSections = new SectionCollection(globalSections);
            Properties = new PropertyItemCollection(properties);
        }

        /// <summary>
        /// Gets or sets the full path. If it's a solution folder, it should just be the name of the folder.
        /// </summary>
        /// <value>The full path.</value>
        public string FullPath { get; set; }

        /// <summary>
        /// Gets all projects that are not folders.
        /// </summary>
        /// <value>The children.</value>
        [NotNull]
        public IEnumerable<Project> Children
        {
            get
            {
                return projects.Where(project => project.GetParentProject(this) == null);
            }
        }

        /// <summary>
        /// Gets the global sections.
        /// </summary>
        /// <value>The global sections.</value>
        public SectionCollection GlobalSections
        {
            get
            {
                return globalSections;
            }
        }

        /// <summary>
        /// Gets the headers.
        /// </summary>
        /// <value>The headers.</value>
        public List<string> Headers
        {
            get
            {
                return headers;
            }
        }

        /// <summary>
        /// Gets all projects.
        /// </summary>
        /// <value>The projects.</value>
        public ProjectCollection Projects
        {
            get
            {
                return projects;
            }
        }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>The properties.</value>
        public PropertyItemCollection Properties { get; private set; }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>Solution.</returns>
        [NotNull]
        public Solution Clone()
        {
            return new Solution(this);
        }

        /// <summary>
        /// Saves this instance to the <see cref="FullPath"/> path.
        /// </summary>
        public void Save()
        {
            SaveAs(FullPath);
        }

        /// <summary>
        /// Saves this instance to the specified path.
        /// </summary>
        /// <param name="solutionPath">The solution path.</param>
        public void SaveAs(string solutionPath)
        {
            // If the solution file already exists, we want to write it only if it has actually changed, to prevent Visual Studio to reload the solution
            if (File.Exists(solutionPath))
            {
                // Read the current version of the solution file.
                string currentVersion;
                StreamReader reader;
                using (reader = new StreamReader(solutionPath))
                {
                    currentVersion = reader.ReadToEnd();
                }
                var memoryStream = new MemoryStream();

                // Write the new version of the solution file in memory
                var writer = new SolutionWriter(memoryStream);
                writer.WriteSolutionFile(this);
                writer.Flush();
                memoryStream.Position = 0;

                // Retrieve the new version of the solution file in a string
                reader = new StreamReader(memoryStream);
                var newVersion = reader.ReadToEnd();
                memoryStream.Close();

                // If the versions are different, actually write the new version on disk
                if (newVersion != currentVersion)
                {
                    using (var streamWriter = new StreamWriter(solutionPath))
                    {
                        streamWriter.Write(newVersion);
                    }
                }
            }
            else
            {
                using (var writer = new SolutionWriter(solutionPath))
                {
                    writer.WriteSolutionFile(this);
                }
            }
        }

        /// <summary>
        /// Loads the solution from the specified file.
        /// </summary>
        /// <param name="solutionFullPath">The solution full path.</param>
        /// <returns>Solution.</returns>
        [NotNull]
        public static Solution FromFile(string solutionFullPath)
        {
            using (var reader = new SolutionReader(solutionFullPath))
            {
                var solution = reader.ReadSolutionFile();
                solution.FullPath = solutionFullPath;
                return solution;
            }
        }

        /// <summary>
        /// Loads the solution from the specified stream.
        /// </summary>
        /// <param name="solutionFullPath">The solution full path.</param>
        /// <param name="stream">The stream.</param>
        /// <returns>Solution.</returns>
        [NotNull]
        public static Solution FromStream(string solutionFullPath, [NotNull] Stream stream)
        {
            using (var reader = new SolutionReader(solutionFullPath, stream))
            {
                var solution = reader.ReadSolutionFile();
                solution.FullPath = solutionFullPath;
                return solution;
            }
        }
    }
}
