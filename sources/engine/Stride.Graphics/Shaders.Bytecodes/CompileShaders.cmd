@echo off
setlocal
set StrideSdkDir=%~dp0..\..\..\..\
set StrideAssetCompiler=%StrideSdkDir%sources\assets\Stride.Core.Assets.CompilerApp\bin\Debug\net472\Stride.Core.Assets.CompilerApp.exe
%StrideAssetCompiler% --platform=Windows --property:RuntimeIdentifier=win --output-path=%~dp0obj\app_data --build-path=%~dp0obj\build_app_data --package-file=Graphics.xkpkg
