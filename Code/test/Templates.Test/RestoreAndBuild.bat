call "%ProgramFiles(x86)%\Microsoft Visual Studio 14.0\Common7\Tools\VsMSBuildCmd.bat"
..\..\..\..\..\..\_tools\nuget.exe restore -OutputDirectory c:\packages

msbuild "%~1" /t:Rebuild /p:Configuration=%3;Platform=%2;AppxPackageSigningEnabled=false;NuGetPackagesDirectory=C:\packages