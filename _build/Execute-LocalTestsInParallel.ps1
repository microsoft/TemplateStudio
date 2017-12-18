
$rootPath = (Split-Path $PSScriptRoot)

$scriptPath = Join-Path $rootPath \_build\ParallelTestExecution.ps1
$testrunnerPath = Join-Path $rootPath \Code\packages\xunit.runner.console.2.2.0\tools\xunit.console.exe
$templateTestLibraryPath = Join-Path $rootPath \Code\test\Templates.Test\bin\Analyze\Microsoft.Templates.Test.dll
$coreTestLibraryPath = Join-Path $rootPath \Code\test\Core.Test\bin\Analyze\Microsoft.Templates.Core.Test.dll 
$uiTestLibraryPath = Join-Path $rootPath \Code\test\UI.Test\bin\Analyze\Microsoft.UI.Test.dll 
$traits = 'ExecutionSet=BuildCodeBehind', 'ExecutionSet=BuildMVVMBasic','ExecutionSet=BuildMVVMLight','ExecutionSet=BuildCaliburnMicro','ExecutionSet=BuildPrism', 'ExecutionSet=BuildStyleCop', 'ExecutionSet=TemplateValidation', 'ExecutionSet=BuildRightClickWithLegacy'
$outputDir = 'C:\temp\testresults'

if (-not (Test-Path $outputDir))
{
    New-Item $outputDir -type Directory 
}

 . $testrunnerPath $coreTestLibraryPath 

 . $testrunnerPath $uiTestLibraryPath 

 . $scriptPath $testrunnerPath $templateTestLibraryPath $traits $outputDir

Write-Host $rootPath
Write-Host $scriptPath