
$rootPath = (Split-Path $PSScriptRoot)

$scriptPath = Join-Path $rootPath \_build\ParallelTestExecution.ps1
$testrunnerPath = Join-Path $env:userprofile \.nuget\packages\xunit.runner.console\2.4.1\tools\net47\xunit.console.exe
$templateTestLibraryPath = Join-Path $rootPath \Code\test\Templates.Test\bin\Analyze\Microsoft.Templates.Test.dll
$coreTestLibraryPath = Join-Path $rootPath \Code\test\Core.Test\bin\Analyze\Microsoft.Templates.Core.Test.dll 
$uiTestLibraryPath = Join-Path $rootPath \Code\test\UI.Test\bin\Analyze\Microsoft.UI.Test.dll 
$traits1 = 'ExecutionSet=BuildStyleCop', 'ExecutionSet=TemplateValidation', 'ExecutionSet=BuildRightClickWithLegacy'
$traits2 = 'ExecutionSet=BuildCodeBehind', 'ExecutionSet=BuildMVVMBasic','ExecutionSet=BuildMVVMLight','ExecutionSet=BuildCaliburnMicro','ExecutionSet=BuildPrism'
$outputDir = 'C:\temp\testresults'

if (-not (Test-Path $outputDir))
{
    New-Item $outputDir -type Directory 
}

 . $testrunnerPath $coreTestLibraryPath 

 . $testrunnerPath $uiTestLibraryPath -notrait "ExecutionSet=ManualOnly" 

 . $scriptPath $testrunnerPath $templateTestLibraryPath $traits1 $outputDir
 . $scriptPath $testrunnerPath $templateTestLibraryPath $traits2 $outputDir


Write-Host $rootPath
Write-Host $scriptPath