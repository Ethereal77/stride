// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using Microsoft.Build.Locator;

using System.IO;
using Stride.Core.IO;
using Stride.Core.Yaml;
using Stride.Core.Assets.Quantum;
using Stride.Core.Assets.Templates;

namespace Stride.Core.Assets.Editor.Components.TemplateDescriptions
{
    /// <summary>
    ///   Represents an <see cref="ITemplateGenerator"/> that will save the session and update the assembly
    ///   references.
    /// </summary>
    /// <remarks>
    ///   An <see cref="AfterSave"/> protected method is provided to do additional work after saving.
    /// </remarks>
    public abstract class SessionTemplateGenerator : TemplateGeneratorBase<SessionTemplateGeneratorParameters>
    {
        private readonly AssetPropertyGraphContainer graphContainer =
            new AssetPropertyGraphContainer(
                new AssetNodeContainer { NodeBuilder = { NodeFactory = new AssetNodeFactory() } });

        // TODO: Move .gitignore content into an external file.
        //  Sadly templates and new games are different. Templates are basically a copy; New games are more
        //  programatically generated. Our best bet to have something easy to mantain is this for now.
        private static readonly string GitIgnore = @"
*.user
*.lock
*.lock.json
.vs/
_ReSharper*
*.suo
*.VC.db
*.vshost.exe
*.manifest
*.sdf
[Bb]in/
obj/
Cache/
";

        private static string GlobalJson(string version) => $"{{ \"sdk\": {{ \"version\": \"{version}\" }} }}";

        public sealed override bool Run(SessionTemplateGeneratorParameters parameters)
        {
            var result = Generate(parameters);
            if (!result)
                return false;

            SaveSession(parameters);

            // Load missing references (we do this after saving)
            // TODO: Better tracking of ProjectReferences (added, removed, etc...)
            parameters.Logger.Verbose("Compiling game assemblies...");
            parameters.Session.UpdateAssemblyReferences(parameters.Logger);
            parameters.Logger.Verbose("Game assemblies compiled...");

            result = AfterSave(parameters).Result;

            return result;
        }

        /// <summary>
        ///   Generates the template.
        /// </summary>
        /// <param name="parameters">The parameters for the template generator.</param>
        /// <returns><c>true</c> if the generation was successful; <c>false</c> otherwise.</returns>
        /// <remarks>
        ///   This method is called by <see cref="Run"/>, and the session is saved afterwards  if the
        ///   generation is successful.
        ///   <para/>
        ///   This method should work in unattended mode and should not ask user for information anymore.
        /// </remarks>
        protected abstract bool Generate(SessionTemplateGeneratorParameters parameters);

        /// <summary>
        ///   Method called after the session has been saved. Override in a subclass to do additional work
        ///   after the session is saved.
        /// </summary>
        /// <param name="parameters">The parameters passed to the template generator.</param>
        /// <returns><c>true</c> if the method succeeded; <c>false</c> otherwise.</returns>
        protected virtual Task<bool> AfterSave(SessionTemplateGeneratorParameters parameters)
        {
            return Task.FromResult(true);
        }

        protected void ApplyMetadata(SessionTemplateGeneratorParameters parameters)
        {
            // Create graphs for all assets in the session
            EnsureGraphs(parameters);

            // Then apply metadata from each asset item to the graph
            foreach (var package in parameters.Session.LocalPackages)
            foreach (var asset in package.Assets)
            {
                var graph = graphContainer.TryGetGraph(asset.Id) ??
                            graphContainer.InitializeAsset(asset, parameters.Logger);

                var overrides = asset.YamlMetadata.RetrieveMetadata(AssetObjectSerializerBackend.OverrideDictionaryKey);

                if (graph != null && overrides != null)
                {
                    graph.RefreshBase();
                    AssetPropertyGraph.ApplyOverrides(graph.RootNode, overrides);
                }
            }
        }

        protected void SaveSession(SessionTemplateGeneratorParameters parameters)
        {
            // Create graphs for all assets in the session
            EnsureGraphs(parameters);

            // Then run a PrepareForSave pass to prepare asset items to be saved (override, object references, etc.)
            foreach (var package in parameters.Session.LocalPackages)
            foreach (var asset in package.Assets)
            {
                var graph = graphContainer.TryGetGraph(asset.Id);
                graph?.PrepareForSave(parameters.Logger, asset);
            }

            // Finally actually save the session.
            parameters.Session.DependencyManager.BeginSavingSession();
            parameters.Session.SourceTracker.BeginSavingSession();
            parameters.Session.Save(parameters.Logger);
            parameters.Session.SourceTracker.EndSavingSession();
            parameters.Session.DependencyManager.EndSavingSession();
        }

        protected void WriteGitIgnore(SessionTemplateGeneratorParameters parameters)
        {
            var fileName = UFile.Combine(parameters.OutputDirectory, ".gitignore");
            File.WriteAllText(fileName.ToWindowsPath(), GitIgnore);
        }

        protected void WriteGlobalJson(SessionTemplateGeneratorParameters parameters)
        {
            if (!RuntimeInformation.FrameworkDescription.StartsWith(".NET Core"))
                return;

            var sdkVersion = MSBuildLocator.QueryVisualStudioInstances()
                .First(vs => vs.DiscoveryType == DiscoveryType.DotNetSdk && vs.Version.Major >= 3).Version;
            var fileName = UFile.Combine(parameters.OutputDirectory, "global.json");
            File.WriteAllText(fileName.ToWindowsPath(), GlobalJson(sdkVersion.ToString()));
        }

        private void EnsureGraphs(SessionTemplateGeneratorParameters parameters)
        {
            foreach (var package in parameters.Session.Packages)
            foreach (var asset in package.Assets)
            {
                if (graphContainer.TryGetGraph(asset.Id) is null)
                    graphContainer.InitializeAsset(asset, parameters.Logger);
            }
        }
    }
}
