![Stride](sources/data/images/Logo/stride-logo-readme.png)
==========================================================

Welcome to the Stride source code repository!

Stride is an open-source **C# game engine** for realistic rendering and an advanced editor (**Game Studio**) that allows
you create and manage the content of your games or applications in a visual and intuitive way.

![Stride Editor](https://stride3d.net/images/external/script-editor.png)

The engine is highly modular and aims at giving game makers more flexibility in their development.

To learn more about Stride, visit [stride3d.net](https://stride3d.net/).

## License

Stride is covered by [MIT](LICENSE.md), unless stated otherwise (i.e. for some files that are copied from other projects).

You can find the list of third party projects [here](THIRD%20PARTY.md).

Contributors need to sign the following [Contribution License Agreement](docs/ContributorLicenseAgreement.md).

## Documentation

Find explanations and information about Stride:

 * [Stride Manual](https://doc.stride3d.net/latest/manual/index.html)
 * [API Reference](https://doc.stride3d.net/latest/api/index.html)
 * [Release Notes](https://doc.stride3d.net/latest/ReleaseNotes/index.html)

## Community

Ask for help or report issues:

 * [Chat with the community on Discord](https://discord.gg/f6aerfE) [![Join the chat at https://discord.gg/f6aerfE](https://img.shields.io/discord/500285081265635328.svg?style=flat&logo=discord&label=discord)](https://discord.gg/f6aerfE)
 * [Discuss topics on our forums](http://forums.stride3d.net/)
 * [Report engine issues](https://github.com/stride3d/stride/issues)
 * [Donate to support the project](https://www.patreon.com/stride3d)
 * [List of Projects made by users](https://github.com/stride3d/stride-community-projects)
 * [Localization](docs/localization.md)

## Building from source

### Prerequisites

1. [Git](https://git-scm.com/downloads) with **Large File Support** selected.

2. [Visual Studio 2019](https://www.visualstudio.com/downloads/) with the following workloads:

  * `.NET desktop development` with `.NET Framework 4.7.2 targeting pack`.

  * `Desktop development with C++` with

    - `Windows 10 SDK (10.0.18362.0)` or later (should be enabled by default).
    - `MSVC v142 - VS2019 C++ x64/x86 build tools (v14.26)` or later (should be enabled by default)
    - `C++/CLI support for v142 build tools (v14.26)` or later (**not enabled by default**).

  * `.NET Core cross-platform development`.

3. [FBX SDK 2019.0 VS2015](https://www.autodesk.com/developer-network/platform-technologies/fbx-sdk-2019-0)

### Build Stride

1. Clone Stride: `git clone https://github.com/Ethereal77/stride.git`.
2. Open `Stride.sln` in the `build` directory with Visual Studio 2019 and build `Stride.GameStudio` (it should be the default startup project).

### Build Stride without Visual Studio

1. Install Visual Studio 2019 Build Tools with the same prerequisites listed above.
2. Add MSBuild's directory to your system's `PATH`.
3. Clone Stride: `git clone https://github.com/Ethereal77/stride.git`.
4. Navigate to the `build` directory with the command prompt, input `dotnet restore Stride.sln` then `dotnet build Stride.sln`.

For .NET 5 make sure that you have the latest .NET 5 SDK and Runtime, navigate to `\sources\targets\Stride.Core.TargetFrameworks.Editor.props` and change `net472` to `net5.0-windows`.

If building failed:
 * If you skipped one of the `Prerequisites` thinking that you already have the latest version, update to the latest anyway just to be sure.
 * Visual Studio might have issues properly building if an outdated version of VS 2017 is present alongside VS 2019. If you want to keep VS 2017 make sure that it is up to date and that you are building Stride through VS 2019.
 * Some changes might require a system reboot, try that if you haven't yet.
 * Make sure that Git and Visual Studio can access the Internet.
 * Close Visual Studio, clear the NuGet cache (in your command prompt `dotnet nuget locals all --clear`), delete the hidden `.vs` folder inside `\build` and the files inside `bin\packages`, kill any MSBuild and other VS processes, build the whole solution then build and run GameStudio.

Do note that test solutions might fail but it should not prevent you from building `Stride.GameStudio`.

### Contribution Guidelines

Please check our [Contributing Guidelines](docs/CONTRIBUTING.md).

### Build Status

|Branch| **master** |
|:--:|:--:|
|Windows D3D11|<a href="https://teamcity.stride3d.net/viewType.html?buildTypeId=Engine_BuildWindowsD3d11&branch=master&guest=1"><img src="https://teamcity.stride3d.net/app/rest/builds/buildType:(id:Engine_BuildWindowsD3d11),branch:master/statusIcon"/></a>
|Windows D3D12|<a href="https://teamcity.stride3d.net/viewType.html?buildTypeId=Engine_BuildWindowsD3d12&branch=master&guest=1"><img src="https://teamcity.stride3d.net/app/rest/builds/buildType:(id:Engine_BuildWindowsD3d12),branch:master/statusIcon"/></a>
|Tests Windows Simple| <a href="https://teamcity.stride3d.net/viewType.html?buildTypeId=Engine_Tests_WindowsSimple&branch=master&guest=1"><img src="https://teamcity.stride3d.net/app/rest/builds/buildType:(id:Engine_Tests_WindowsSimple),branch:master/statusIcon"/></a>
|Tests Windows D3D11|<a href="https://teamcity.stride3d.net/viewType.html?buildTypeId=Engine_Tests_WindowsD3D11&branch=master&guest=1"><img src="https://teamcity.stride3d.net/app/rest/builds/buildType:(id:Engine_Tests_WindowsD3D11),branch:master/statusIcon"/></a> 
