:: Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
:: Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
:: Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

@echo OFF
where /q git
IF ERRORLEVEL 0 (
  @set GIT_CMD=git
) ELSE IF EXIST "%ProgramFiles%\Git\Bin\git.exe" (
  @set GIT_CMD="%ProgramFiles%\Git\Bin\git.exe"
) ELSE IF EXIST "%ProgramFiles(x86)%\Git\Bin\git.exe" (
  @set GIT_CMD="%ProgramFiles(x86)%\Git\Bin\git.exe"
) ELSE EXIT /B 1
