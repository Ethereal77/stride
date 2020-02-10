call "%ProgramFiles(x86)%\Microsoft Visual Studio 12.0\vc\vcvarsall.bat" x86

ildasm /all /out:BulletSharp.NetStandard.il BulletSharp.NetStandard.dll

@echo "Please patch BulletSharp.NetStandard.il Xenko.Core.Mathematics reference with .publickeytoken = ( BA CA CC 89 C3 B6 D5 56 )"
pause

mkdir Signed
ilasm /dll /key:..\..\build\xenko.public.snk /output:Signed\BulletSharp.NetStandard.dll BulletSharp.NetStandard.il
