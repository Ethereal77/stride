call "%ProgramFiles(x86)%\Microsoft Visual Studio 12.0\vc\vcvarsall.bat" x86

ildasm /all /out:Windows\Debug\SharpFont.il Windows\Debug\SharpFont.dll
ilasm /dll /debug /key:..\..\build\paradox.snk  Windows\Debug\SharpFont.il

ildasm /all /out:Windows\Release\SharpFont.il Windows\Release\SharpFont.dll
ilasm /dll /pdb /key:..\..\build\paradox.snk Windows\Release\SharpFont.il

del /s .\*.il
del /s .\*.res
