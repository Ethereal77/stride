// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using EnvDTE;
using EnvDTE80;

using Microsoft.Build.Evaluation;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;

using NShader;

using Stride.VisualStudio.BuildEngine;
using Stride.VisualStudio.Commands;
using Stride.VisualStudio.Shaders;

using Task = System.Threading.Tasks.Task;

namespace Stride.VisualStudio
{
    //  Quick and temporary VS package to allow platform switch for Stride.
    //  This code needs to be largely refactored and correctly designed.
    //  - alex
    //
    //     This is the class that implements the package exposed by this assembly.
    //     The minimum requirement for a class to be considered a valid package for Visual Studio
    //     is to implement the IVsPackage interface and register itself with the shell.
    //     This package uses the helper classes defined inside the Managed Package Framework (MPF)
    //     to do it: it derives from the Package class that provides the implementation of the
    //     IVsPackage interface and uses the registration attributes defined in the framework to
    //     register itself and its components with the shell.

    /// <summary>
    ///  Visual Studio Package to provide several contextual actions for Stride, syntax highlighting for SDSL / SDFX
    ///  and Stride Shader files design-time code generation.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is a package.
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    // This attribute is used to register the information needed to show this package in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", Version, IconResourceID = 400)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    // This attribute registers a tool window exposed by this package.
    //[ProvideToolWindow(typeof (MyToolWindow))]
    [Guid(GuidList.guidStride_VisualStudio_PackagePkgString)]
    // Stride Shader LanguageService
    [ProvideService(typeof(NShaderLanguageService), ServiceName = "Stride Shader Language Service", IsAsyncQueryable = true)]
    [ProvideLanguageService(typeof(NShaderLanguageService),
                            "Stride Shader Language",
                            0,
                            EnableCommenting = true,
                            EnableFormatSelection = true,
                            EnableLineNumbers = true,
                            DefaultToInsertSpaces = true,
                            CodeSense = true)]
    [ProvideLanguageExtension(typeof(NShaderLanguageService), NShaderSupportedExtensions.Stride_Shader)]
    [ProvideLanguageExtension(typeof(NShaderLanguageService), NShaderSupportedExtensions.Stride_Effect)]
    // Stride C# Effect Code Generator
    [CodeGeneratorRegistration(typeof(EffectCodeFileGenerator), EffectCodeFileGenerator.InternalName, GuidList.vsContextGuidVCSProject, GeneratorRegKeyName = ".sdfx")]
    [CodeGeneratorRegistration(typeof(EffectCodeFileGenerator), EffectCodeFileGenerator.DisplayName, GuidList.vsContextGuidVCSProject, GeneratorRegKeyName = EffectCodeFileGenerator.InternalName, GeneratesDesignTimeSource = true, GeneratesSharedDesignTimeSource = true)]
    [CodeGeneratorRegistration(typeof(EffectCodeFileGenerator), EffectCodeFileGenerator.InternalName, GuidList.vsContextGuidVCSNewProject, GeneratorRegKeyName = ".sdfx")]
    [CodeGeneratorRegistration(typeof(EffectCodeFileGenerator), EffectCodeFileGenerator.DisplayName, GuidList.vsContextGuidVCSNewProject, GeneratorRegKeyName = EffectCodeFileGenerator.InternalName, GeneratesDesignTimeSource = true, GeneratesSharedDesignTimeSource = true)]
    // Stride C# Shader Key Generator
    [CodeGeneratorRegistration(typeof(ShaderKeyFileGenerator), ShaderKeyFileGenerator.InternalName, GuidList.vsContextGuidVCSProject, GeneratorRegKeyName = ".sdsl")]
    [CodeGeneratorRegistration(typeof(ShaderKeyFileGenerator), ShaderKeyFileGenerator.DisplayName, GuidList.vsContextGuidVCSProject, GeneratorRegKeyName = ShaderKeyFileGenerator.InternalName, GeneratesDesignTimeSource = true, GeneratesSharedDesignTimeSource = true)]
    [CodeGeneratorRegistration(typeof(ShaderKeyFileGenerator), ShaderKeyFileGenerator.InternalName, GuidList.vsContextGuidVCSNewProject, GeneratorRegKeyName = ".sdsl")]
    [CodeGeneratorRegistration(typeof(ShaderKeyFileGenerator), ShaderKeyFileGenerator.DisplayName, GuidList.vsContextGuidVCSNewProject, GeneratorRegKeyName = ShaderKeyFileGenerator.InternalName, GeneratesDesignTimeSource = true, GeneratesSharedDesignTimeSource = true)]
    // Temporarily force load for easier debugging
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExists_string, PackageAutoLoadFlags.BackgroundLoad)]
    public sealed class StridePackage : AsyncPackage, IOleComponent
    {
        public const string Version = "2.0";

        private readonly Dictionary<EnvDTE.Project, string> previousProjectPlatforms = new Dictionary<EnvDTE.Project, string>();
        private EnvDTE.Project currentStartupProject;
        private bool configurationLock;

        private SolutionEventsListener solutionEventsListener;
        private ErrorListProvider errorListProvider;
        private uint m_componentID;

        /// <summary>
        ///   Default constructor of the package.
        /// </summary>
        public StridePackage()
        {
            // The initialization code that does not require any Visual Studio service can go here. At this point the package object is
            // created but not sited yet inside Visual Studio environment.

            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", ToString()));
        }

        #region Package Members

        /// <summary>
        ///   Initializes the package; this method is called right after the package is sited, so this is the place
        ///   where all the initialization that rely on services provided by Visual Studio is executed.
        /// </summary>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering InitializeAsync() of: {0}", ToString()));
            await base.InitializeAsync(cancellationToken, progress);

            IDEBuildLogger.UserRegistryRoot = UserRegistryRoot;

            // Switching to main thread to use GetService RPC and cast to service interface (which may involve COM operations)
            // Note: most of our work is not supposed to be heavy, mostly registration of services and callbacks
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            solutionEventsListener = new SolutionEventsListener(this);
            solutionEventsListener.BeforeSolutionClosed += solutionEventsListener_BeforeSolutionClosed;
            solutionEventsListener.AfterSolutionOpened += solutionEventsListener_AfterSolutionOpened;
            solutionEventsListener.AfterActiveConfigurationChange += SolutionEventsListener_AfterActiveConfigurationChange;
            solutionEventsListener.StartupProjectChanged += SolutionEventsListener_OnStartupProjectChanged;

            // Register the C# language service
            // Inspired by https://github.com/IInspectable/Nav.Language.Extensions/commit/08af3d897afac5a54975660fa03f4b629da405e1#diff-b73c0f368f242625f60cfad9cc11f2d5R88
            AddService(typeof(NShaderLanguageService), async (container, cancelToken, type) =>
                {
                    await JoinableTaskFactory.SwitchToMainThreadAsync(cancelToken);
                    errorListProvider = new ErrorListProvider(this)
                    {
                        ProviderGuid = new Guid("ad1083c5-32ad-403d-af3d-32fee7abbdf1"),
                        ProviderName = "Stride Shading Language"
                    };

                    var langService = new NShaderLanguageService(errorListProvider);
                    langService.SetSite(this);
                    // Make sure to initialize colors before registering!
                    langService.InitializeColors();
                    return langService;
                },
                promote: true);

            // Add our command handlers for menu (commands must exist in the .vsct file)
            if (await GetServiceAsync(typeof(IMenuCommandService)) is OleMenuCommandService menuCommandService)
            {
                StrideCommands.ServiceProvider = this;
                StrideCommands.RegisterCommands(menuCommandService);
            }

            // Register a timer to call our language service during
            // idle periods.
            if (m_componentID == 0 && GetService(typeof(SOleComponentManager)) is IOleComponentManager componentManager)
            {
                var crInfo = new OLECRINFO[1];
                crInfo[0].cbSize = (uint) Marshal.SizeOf(typeof(OLECRINFO));
                crInfo[0].grfcrf = (uint) _OLECRF.olecrfNeedIdleTime |
                                   (uint) _OLECRF.olecrfNeedPeriodicIdleTime;
                crInfo[0].grfcadvf = (uint) _OLECADVF.olecadvfModal |
                                     (uint) _OLECADVF.olecadvfRedrawOff |
                                     (uint) _OLECADVF.olecadvfWarningsOff;
                crInfo[0].uIdleTimeInterval = 1000;
                componentManager.FRegisterComponent(this, crInfo, out m_componentID);
            }

            // If there's already a solution loaded, process it
            var dte = (DTE) GetService(typeof(DTE));
            if (dte.Solution.IsOpen)
                await InitializeCommandProxy();

            // Go back to async thread
            await TaskScheduler.Default;
        }

        public static bool IsProjectExecutable(EnvDTE.Project project)
        {
            var buildProjects = ProjectCollection.GlobalProjectCollection.GetLoadedProjects(project.FileName);
            return buildProjects.Any(x => x.GetPropertyValue("StrideProjectType") == "Executable");
        }

        public static string GetProjectPlatform(EnvDTE.Project project)
        {
            var buildProjects = ProjectCollection.GlobalProjectCollection.GetLoadedProjects(project.FileName);
            return buildProjects.Select(x => x.GetPropertyValue("StridePlatform")).FirstOrDefault(x => !string.IsNullOrEmpty(x));
        }

        private void SolutionEventsListener_OnStartupProjectChanged(IVsHierarchy hierarchy)
        {
            if (configurationLock || hierarchy is null)
                return;

            currentStartupProject = VsHelper.ToDteProject(hierarchy);

            UpdateConfigurationFromStartupProject();
        }

        private void SolutionEventsListener_AfterActiveConfigurationChange(IVsCfg oldConfiguration, IVsCfg newConfiguration)
        {
            if (configurationLock || newConfiguration is null)
                return;

            UpdateStartupProjectFromConfiguration();
        }

        private void UpdateConfigurationFromStartupProject()
        {
            if (currentStartupProject is null)
                return;

            var projectPlatform = GetProjectPlatform(currentStartupProject);
            var dte = (DTE) GetService(typeof(DTE));
            var activeConfiguration = dte.Solution.SolutionBuild.ActiveConfiguration;

            var hasPreviousPlatform = previousProjectPlatforms.TryGetValue(currentStartupProject, out string startupPlatform);

            SolutionConfiguration2 newConfiguration = null;

            bool foundPreferredPlatform = false;
            foreach (SolutionConfiguration2 configuration in dte.Solution.SolutionBuild.SolutionConfigurations)
            {
                if (foundPreferredPlatform)
                    break;

                if (configuration.Name != activeConfiguration.Name)
                    continue;

                if ((projectPlatform == null) || !configuration.PlatformName.StartsWith(projectPlatform))
                    continue;

                foreach (SolutionContext context in configuration.SolutionContexts)
                {
                    if (!context.ShouldBuild || context.ProjectName != currentStartupProject.UniqueName)
                        continue;

                    if (hasPreviousPlatform && context.PlatformName != startupPlatform)
                        continue;

                    newConfiguration = configuration;

                    if (IsPreferredPlatform(context.PlatformName))
                    {
                        foundPreferredPlatform = true;
                        break;
                    }
                }
            }

            if (newConfiguration != null && newConfiguration != activeConfiguration)
            {
                try
                {
                    configurationLock = true;
                    newConfiguration.Activate();
                }
                finally
                {
                    configurationLock = false;
                }
            }
        }

        private static bool IsPreferredPlatform(string platformName)
        {
            // Prefer non-ARM
            return (platformName != "ARM");
        }

        private void UpdateStartupProjectFromConfiguration()
        {
            var solution = (IVsSolution)GetGlobalService(typeof(IVsSolution));
            var buildManager = (IVsSolutionBuildManager)GetGlobalService(typeof(IVsSolutionBuildManager));
            var dte = (DTE)GetService(typeof(DTE));

            foreach (SolutionContext context in dte.Solution.SolutionBuild.ActiveConfiguration.SolutionContexts)
            {
                if (!context.ShouldBuild)
                    continue;

                foreach (var project in VsHelper.GetDteProjectsInSolution(solution))
                {
                    if (context.ProjectName != project.UniqueName || !IsProjectExecutable(project))
                        continue;

                    var startupProjects = (object[])dte.Solution.SolutionBuild.StartupProjects;
                    if (!startupProjects.Cast<string>().Contains(project.UniqueName))
                    {
                        buildManager.set_StartupProject(VsHelper.ToHierarchy(project));
                    }

                    previousProjectPlatforms[project] = context.PlatformName;
                    return;
                }
            }
        }

        private async void solutionEventsListener_AfterSolutionOpened()
        {
            await InitializeCommandProxy();
        }

        private void solutionEventsListener_BeforeSolutionClosed()
        {
            // Disable UIContext (this will hide Stride menus)
            UpdateCommandVisibilityContext(false);

            StrideCommandsProxy.CloseSolution();
        }

        private async System.Threading.Tasks.Task InitializeCommandProxy()
        {
            // Initialize the command proxy from the current solution's package
            var dte = (DTE)GetService(typeof(DTE));
            var solutionPath = dte.Solution.FullName;

            // Get General Output pane (for error logging)
            var generalOutputPane = GetGeneralOutputPane();

            try
            {
                StrideCommandsProxy.SetSolution(solutionPath);
                var stridePackageInfo = await StrideCommandsProxy.FindStrideSdkDir(solutionPath);
                StrideCommandsProxy.SetPackageInfo(stridePackageInfo);
                if (stridePackageInfo.LoadedVersion is null)
                    return;

                // Enable UIContext depending on wheter it is a Stride project. This will show or hide Stride menus.
                var isStrideSolution = stridePackageInfo.LoadedVersion != null;
                UpdateCommandVisibilityContext(isStrideSolution);

                // If a package is associated with the solution, check if the correct version was found
                if (stridePackageInfo.ExpectedVersion != null && stridePackageInfo.ExpectedVersion != stridePackageInfo.LoadedVersion)
                {
                    if (stridePackageInfo.ExpectedVersion < StrideCommandsProxy.MinimumVersion)
                    {
                        // The package version is deprecated
                        generalOutputPane.OutputStringThreadSafe($"Could not initialize Stride extension for package with version {stridePackageInfo.ExpectedVersion}. Versions earlier than {StrideCommandsProxy.MinimumVersion} are not supported. Loading latest version {stridePackageInfo.LoadedVersion} instead.\r\n");
                        generalOutputPane.Activate();
                    }
                    else if (stridePackageInfo.LoadedVersion is null)
                    {
                        // No version found
                        generalOutputPane.OutputStringThreadSafe("Could not find Stride SDK directory.");
                        generalOutputPane.Activate();

                        // Don't try to create any services
                        return;
                    }
                    else
                    {
                        // The package version was not found
                        generalOutputPane.OutputStringThreadSafe($"Could not find SDK directory for Stride version {stridePackageInfo.ExpectedVersion}. Loading latest version {stridePackageInfo.LoadedVersion} instead.\r\n");
                        generalOutputPane.Activate();
                    }
                }

                // Preinitialize the parser in a separate thread
                var thread = new System.Threading.Thread(
                    () =>
                    {
                        try
                        {
                            StrideCommandsProxy.GetProxy();
                        }
                        catch (Exception ex)
                        {
                            generalOutputPane.OutputStringThreadSafe($"Error Initializing Stride Language Service: {ex.InnerException ?? ex}\r\n");
                            generalOutputPane.Activate();
                        }
                    });
                thread.Start();
            }
            catch (Exception ex)
            {
                // Do not crash VS Plugin if something fails
                generalOutputPane.OutputStringThreadSafe($"Error initializing Stride command proxy: {ex}\r\n");
                generalOutputPane.Activate();

                return;
            }
        }

        private void UpdateCommandVisibilityContext(bool enabled)
        {
            IVsMonitorSelection selectionMonitor = (IVsMonitorSelection) GetService(typeof(SVsShellMonitorSelection));

            var cmdSet = GuidList.guidStride_VisualStudio_PackageCmdSet;
            if (selectionMonitor.GetCmdUIContextCookie(ref cmdSet, out uint cmdUIContextStride) == VSConstants.S_OK)
                selectionMonitor.SetCmdUIContext(cmdUIContextStride, enabled ? 1 : 0);
        }

        protected override void Dispose(bool disposing)
        {
            if (m_componentID != 0)
            {
                if (GetService(typeof(SOleComponentManager)) is IOleComponentManager componentManager)
                {
                    componentManager.FRevokeComponent(m_componentID);
                }
                m_componentID = 0;
            }

            base.Dispose(disposing);
        }

        private IVsOutputWindowPane GetGeneralOutputPane()
        {
            var outputWindow = (IVsOutputWindow) GetGlobalService(typeof(SVsOutputWindow));

            // Get Output pane
            Guid generalPaneGuid = VSConstants.GUID_OutWindowGeneralPane;
            outputWindow.CreatePane(ref generalPaneGuid, "General", 1, 0);
            outputWindow.GetPane(ref generalPaneGuid, out IVsOutputWindowPane pane);
            return pane;
        }

        #endregion

        #region IOleComponent Members

        public int FDoIdle(uint grfidlef)
        {
            bool bPeriodic = (grfidlef & (uint) _OLEIDLEF.oleidlefPeriodic) != 0;

            // Use typeof(TestLanguageService) because we need to reference the GUID for our language service.
            if (GetService(typeof(NShaderLanguageService)) is LanguageService service)
            {
                service.OnIdle(bPeriodic);
            }
            return 0;
        }

        public int FContinueMessageLoop(uint uReason, IntPtr pvLoopData, MSG[] pMsgPeeked) => 1;

        public int FPreTranslateMessage(MSG[] pMsg) => 0;

        public int FQueryTerminate(int fPromptUser) => 1;

        public int FReserved1(uint dwReserved, uint message, IntPtr wParam, IntPtr lParam) => 1;

        public IntPtr HwndGetWindow(uint dwWhich, uint dwReserved) => IntPtr.Zero;

        public void OnActivationChange(IOleComponent pic, int fSameComponent, OLECRINFO[] pcrinfo, int fHostIsActivating, OLECHOSTINFO[] pchostinfo, uint dwReserved) { }

        public void OnAppActivate(int fActive, uint dwOtherThreadID) { }

        public void OnEnterState(uint uStateID, int fEnter) { }

        public void OnLoseActivation() { }

        public void Terminate() { }

        #endregion
    }
}
