@echo off

echo Processing Runtime
..\sources\tools\Xenko.ProjectGenerator\bin\Debug\net472\Xenko.ProjectGenerator.exe solution Xenko.sln -o Xenko.Runtime.sln -p Windows
echo.
