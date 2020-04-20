@echo off

echo Processing Runtime
..\sources\tools\Stride.ProjectGenerator\bin\Debug\net472\Stride.ProjectGenerator.exe solution Stride.sln -o Stride.Runtime.sln -p Windows
echo.
