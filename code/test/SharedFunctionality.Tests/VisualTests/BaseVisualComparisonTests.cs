// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Templates.Core.Helpers;
using WindowsTestHelpers;

namespace Microsoft.Templates.Test.Build
{
    public class BaseVisualComparisonTests : BaseGenAndBuildTests
    {
        public BaseVisualComparisonTests(GenerationFixture fixture)
            : base(fixture)
        {
        }

        protected (bool Success, List<string> TextOutput) RunWinAppDriverTests((string projectFolder, string imagesFolder) testProjectDetails)
        {
            var result = false;

            WinAppDriverHelper.StartIfNotRunning();

            var buildOutput = Path.Combine(testProjectDetails.projectFolder, "bin", "Debug", "AutomatedUITests.dll");
            var runTestsScript = $"& \"{GetVstestsConsoleExePath()}\" \"{buildOutput}\" ";

            // Test failures will be treated as an error but they are handled below
            var output = ExecutePowerShellScript(runTestsScript, outputOnError: true);

            var outputText = new List<string>();

            var testCount = -1;
            var passCount = -1;

            foreach (var outputLine in output)
            {
                var outputLineString = outputLine.ToString();

                outputText.Add(outputLineString);

                if (outputLineString.StartsWith("Total tests: ", StringComparison.OrdinalIgnoreCase))
                {
                    testCount = int.Parse(outputLineString.Substring(12).Trim());
                }
                else if (outputLineString.Trim().StartsWith("Passed: ", StringComparison.OrdinalIgnoreCase))
                {
                    passCount = int.Parse(outputLineString.Trim().Substring(7).Trim());

                    if (testCount > 0)
                    {
                        result = testCount == passCount;
                    }
                }
            }

            WinAppDriverHelper.StopIfRunning();

            return (result, outputText);
        }

        protected (string projectFolder, string imagesFolder) SetUpTestProjectForInitialScreenshotComparison(VisualComparisonTestDetails app1Details, VisualComparisonTestDetails app2Details, string areasOfImageToExclude = null, int noClickCount = 0, bool longPause = false)
        {
            var rootFolder = $"{Path.GetPathRoot(Environment.CurrentDirectory)}UIT\\VIS\\{DateTime.Now:dd_HHmmss}\\";
            var projectFolder = Path.Combine(rootFolder, "TestProject");
            var imagesFolder = Path.Combine(rootFolder, "Images");

            Fs.EnsureFolderExists(rootFolder);
            Fs.EnsureFolderExists(projectFolder);
            Fs.EnsureFolderExists(imagesFolder);

            // Copy base project
            Fs.CopyRecursive(@".\VisualTests\TestProjectSource", projectFolder, overwrite: true);

            // enable appropriate test
            var projFileName = Path.Combine(projectFolder, "AutomatedUITests.csproj");

            var projFileContents = File.ReadAllText(projFileName);

            var newProjFileContents = projFileContents.Replace(
                @"<!--<Compile Include=""Tests\LaunchBothAppsAndCompareInitialScreenshots.cs"" />-->",
                @"<Compile Include=""Tests\LaunchBothAppsAndCompareInitialScreenshots.cs"" />");

            File.WriteAllText(projFileName, newProjFileContents, Encoding.UTF8);

            // set AppInfo values
            var appInfoFileName = Path.Combine(projectFolder, "TestAppInfo.cs");

            var appInfoFileContents = File.ReadAllText(appInfoFileName);

            var newAppInfoFileContents = appInfoFileContents
                .Replace("***APP-PFN-1-GOES-HERE***", $"{app1Details.PackageFamilyName}!App")
                .Replace("***APP-PFN-2-GOES-HERE***", $"{app2Details.PackageFamilyName}!App")
                .Replace("***APP-NAME-1-GOES-HERE***", app1Details.ProjectName)
                .Replace("***APP-NAME-2-GOES-HERE***", app2Details.ProjectName)
                .Replace("***FOLDER-GOES-HERE***", imagesFolder)
                .Replace("public const int NoClickCount = 0;", $"public const int NoClickCount = {noClickCount};");

            if (longPause)
            {
                newAppInfoFileContents = newAppInfoFileContents.Replace("public const bool LongPauseAfterLaunch = false;", "public const bool LongPauseAfterLaunch = true;");
            }

            if (!string.IsNullOrWhiteSpace(areasOfImageToExclude))
            {
                newAppInfoFileContents = newAppInfoFileContents.Replace("new ImageComparer.ExclusionArea[0]", areasOfImageToExclude);
            }

            File.WriteAllText(appInfoFileName, newAppInfoFileContents, Encoding.UTF8);

            // build test project
            var restoreNugetScript = $"& \"{projectFolder}\\nuget.exe\" restore \"{projectFolder}\\AutomatedUITests.csproj\" -PackagesDirectory \"{projectFolder}\\Packages\"";
            ExecutePowerShellScript(restoreNugetScript);
            var buildSolutionScript = $"& \"{GetMsBuildExePath()}\" \"{projectFolder}\\AutomatedUITests.sln\" /t:Rebuild /p:RestorePackagesPath=\"{projectFolder}\\Packages\" /p:Configuration=Debug /p:Platform=\"Any CPU\"";
            ExecutePowerShellScript(buildSolutionScript);

            return (projectFolder, imagesFolder);
        }

        protected (string projectFolder, string imagesFolder) SetUpTestProjectForAllNavViewPagesComparison(VisualComparisonTestDetails app1Details, VisualComparisonTestDetails app2Details, Dictionary<string, string> pageAreasToExclude = null)
        {
            var rootFolder = $"{Path.GetPathRoot(Environment.CurrentDirectory)}UIT\\VIS\\{DateTime.Now:dd_HHmmss}\\";
            var projectFolder = Path.Combine(rootFolder, "TestProject");
            var imagesFolder = Path.Combine(rootFolder, "Images");

            Fs.EnsureFolderExists(rootFolder);
            Fs.EnsureFolderExists(projectFolder);
            Fs.EnsureFolderExists(imagesFolder);

            // Copy base project
            Fs.CopyRecursive(@".\VisualTests\TestProjectSource", projectFolder, overwrite: true);

            // enable appropriate test
            var projFileName = Path.Combine(projectFolder, "AutomatedUITests.csproj");

            var projFileContents = File.ReadAllText(projFileName);

            var newProjFileContents = projFileContents.Replace(
                @"<!--<Compile Include=""Tests\LaunchBothAppsAndCompareAllNavViewPages.cs"" />-->",
                @"<Compile Include=""Tests\LaunchBothAppsAndCompareAllNavViewPages.cs"" />");

            File.WriteAllText(projFileName, newProjFileContents, Encoding.UTF8);

            // set AppInfo values
            var appInfoFileName = Path.Combine(projectFolder, "TestAppInfo.cs");

            var appInfoFileContents = File.ReadAllText(appInfoFileName);

            var newAppInfoFileContents = appInfoFileContents
                .Replace("***APP-PFN-1-GOES-HERE***", $"{app1Details.PackageFamilyName}!App")
                .Replace("***APP-PFN-2-GOES-HERE***", $"{app2Details.PackageFamilyName}!App")
                .Replace("***APP-NAME-1-GOES-HERE***", app1Details.ProjectName)
                .Replace("***APP-NAME-2-GOES-HERE***", app2Details.ProjectName)
                .Replace("***FOLDER-GOES-HERE***", imagesFolder);

            if (pageAreasToExclude != null)
            {
                var replacement = string.Empty;

                foreach (var exclusion in pageAreasToExclude)
                {
                    replacement += $" {{ \"{exclusion.Key}\", {exclusion.Value} }},{Environment.NewLine}";
                }

                newAppInfoFileContents =
                    newAppInfoFileContents.Replace(
                        "PageSpecificExclusions = new Dictionary<string, ImageComparer.ExclusionArea>();",
                        $"PageSpecificExclusions = new Dictionary<string, ImageComparer.ExclusionArea>{{{replacement}}};");
            }

            File.WriteAllText(appInfoFileName, newAppInfoFileContents, Encoding.UTF8);

            // build test project
            var restoreNugetScript = $"& \"{projectFolder}\\nuget.exe\" restore \"{projectFolder}\\AutomatedUITests.csproj\" -PackagesDirectory \"{projectFolder}\\Packages\"";
            ExecutePowerShellScript(restoreNugetScript);
            var buildSolutionScript = $"& \"{GetMsBuildExePath()}\" \"{projectFolder}\\AutomatedUITests.sln\" /t:Rebuild /p:RestorePackagesPath=\"{projectFolder}\\Packages\" /p:Configuration=Debug /p:Platform=\"Any CPU\"";
            ExecutePowerShellScript(buildSolutionScript);

            return (projectFolder, imagesFolder);
        }

        protected (string projectFolder, string imagesFolder) SetUpTestProjectForAllMenuBarPagesComparison(VisualComparisonTestDetails app1Details, VisualComparisonTestDetails app2Details, Dictionary<string, string> pageAreasToExclude = null)
        {
            var rootFolder = $"{Path.GetPathRoot(Environment.CurrentDirectory)}UIT\\VIS\\{DateTime.Now:dd_HHmmss}\\";
            var projectFolder = Path.Combine(rootFolder, "TestProject");
            var imagesFolder = Path.Combine(rootFolder, "Images");

            Fs.EnsureFolderExists(rootFolder);
            Fs.EnsureFolderExists(projectFolder);
            Fs.EnsureFolderExists(imagesFolder);

            // Copy base project
            Fs.CopyRecursive(@".\VisualTests\TestProjectSource", projectFolder, overwrite: true);

            // enable appropriate test
            var projFileName = Path.Combine(projectFolder, "AutomatedUITests.csproj");

            var projFileContents = File.ReadAllText(projFileName);

            var newProjFileContents = projFileContents.Replace(
                @"<!--<Compile Include=""Tests\LaunchBothAppsAndCompareAllMenuBarPages.cs"" />-->",
                @"<Compile Include=""Tests\LaunchBothAppsAndCompareAllMenuBarPages.cs"" />");

            File.WriteAllText(projFileName, newProjFileContents, Encoding.UTF8);

            // set AppInfo values
            var appInfoFileName = Path.Combine(projectFolder, "TestAppInfo.cs");

            var appInfoFileContents = File.ReadAllText(appInfoFileName);

            var newAppInfoFileContents = appInfoFileContents
                .Replace("***APP-PFN-1-GOES-HERE***", $"{app1Details.PackageFamilyName}!App")
                .Replace("***APP-PFN-2-GOES-HERE***", $"{app2Details.PackageFamilyName}!App")
                .Replace("***APP-NAME-1-GOES-HERE***", app1Details.ProjectName)
                .Replace("***APP-NAME-2-GOES-HERE***", app2Details.ProjectName)
                .Replace("***FOLDER-GOES-HERE***", imagesFolder);

            if (pageAreasToExclude != null)
            {
                var replacement = string.Empty;

                foreach (var exclusion in pageAreasToExclude)
                {
                    replacement += $" {{ \"{exclusion.Key}\", {exclusion.Value} }},{Environment.NewLine}";
                }

                newAppInfoFileContents =
                    newAppInfoFileContents.Replace(
                        "PageSpecificExclusions = new Dictionary<string, ImageComparer.ExclusionArea>();",
                        $"PageSpecificExclusions = new Dictionary<string, ImageComparer.ExclusionArea>{{{replacement}}};");
            }

            File.WriteAllText(appInfoFileName, newAppInfoFileContents, Encoding.UTF8);

            // build test project
            var restoreNugetScript = $"& \"{projectFolder}\\nuget.exe\" restore \"{projectFolder}\\AutomatedUITests.csproj\" -PackagesDirectory \"{projectFolder}\\Packages\"";
            ExecutePowerShellScript(restoreNugetScript);
            var buildSolutionScript = $"& \"{GetMsBuildExePath()}\" \"{projectFolder}\\AutomatedUITests.sln\" /t:Rebuild /p:RestorePackagesPath=\"{projectFolder}\\Packages\" /p:Configuration=Debug /p:Platform=\"Any CPU\"";
            ExecutePowerShellScript(buildSolutionScript);

            return (projectFolder, imagesFolder);
        }

        protected void RemoveCertificate(string certificatePath)
        {
            var uninstallCertificateScript = $"$dump = certutil.exe -dump {certificatePath}" + @"
ForEach ($i in $dump)
{
    if ($i.StartsWith(""Serial Number: ""))
            {
                $serialNumber = ($i -split ""Serial Number: "")[1]
                certutil -delstore TrustedPeople $serialNumber
                break
            }
        }";

            ExecutePowerShellScript(uninstallCertificateScript);
        }

        protected void UninstallAppx(string packageFullName)
        {
            ExecutePowerShellScript($"Remove-AppxPackage -Package {packageFullName}");
        }

        protected async Task<VisualComparisonTestDetails> SetUpProjectForUiTestComparisonAsync(string language, string projectType, string framework, IEnumerable<string> genIdentities, bool lastPageIsHome = false, bool createForScreenshots = false)
        {
            var result = new VisualComparisonTestDetails();

            var baseSetup = await SetUpComparisonProjectAsync(language, projectType, framework, genIdentities, lastPageIsHome: lastPageIsHome, includeMultipleInstances: false);

            result.ProjectName = baseSetup.ProjectName;

            if (createForScreenshots)
            {
                var pages = genIdentities.ToArray();

                if (pages.Length == 1)
                {
                    var page = pages.First();

                    if (page == "wts.Page.MediaPlayer")
                    {
                        // Change auto-play to false so not trying to compare images of screen-shot with video playing
                        ReplaceInFiles("AutoPlay=\"True\"", "AutoPlay=\"False\"", baseSetup.ProjectPath, "*.xaml");
                    }
                }
            }

            // So building release version is fast
            ChangeProjectToNotUseDotNetNativeToolchain(baseSetup, language);

            ////Build solution in release mode  // Building in release mode creates the APPX and certificate files we need
            var solutionFile = $"{baseSetup.ProjectPath}\\{baseSetup.ProjectName}.sln";
            var buildSolutionScript = $"& \"{GetMsBuildExePath()}\" \"{solutionFile}\" /t:Restore,Rebuild /p:RestorePackagesPath=\"C:\\Packs\" /p:Configuration=Release /p:Platform=x86";
            ExecutePowerShellScript(buildSolutionScript);

            result.CertificatePath = InstallCertificate(baseSetup);

            // install appx
            var appxFile = $"{baseSetup.ProjectPath}\\{baseSetup.ProjectName}\\AppPackages\\{baseSetup.ProjectName}_1.0.0.0_x86_Test\\{baseSetup.ProjectName}_1.0.0.0_x86.msix";
            ExecutePowerShellScript($"Add-AppxPackage -Path {appxFile}");

            // get app package name
            var manifest = new XmlDocument();
            manifest.Load($"{baseSetup.ProjectPath}\\{baseSetup.ProjectName}\\Package.appxmanifest");
            var packageName = manifest.GetElementsByTagName("Package")[0].FirstChild.Attributes["Name"].Value;

            // get details from appx install
            var getPackageNamesScript = $"$PackageName = \"{packageName}\"" + @"
$packageDetails = Get-AppxPackage -Name $PackageName

$packageFamilyName = $packageDetails.PackageFamilyName
$packageFullName = $packageDetails.PackageFullName

Write-Output $packageFamilyName
Write-Output $packageFullName";

            var response = ExecutePowerShellScript(getPackageNamesScript);

            result.PackageFamilyName = response[0].ToString();
            result.PackageFullName = response[1].ToString();

            return result;
        }

        protected async Task<(string ProjectPath, string ProjectName)> SetUpWpfProjectForUiTestComparisonAsync(string language, string projectType, string framework, IEnumerable<string> genIdentities, bool lastPageIsHome = false)
        {
            var baseSetup = await SetUpWpfComparisonProjectAsync(language, projectType, framework, genIdentities);

            // So building release version is fast
            ChangeProjectToNotUseDotNetNativeToolchain(baseSetup, language);

            ////Build solution in release mode  // Building in release mode creates the APPX and certificate files we need
            var solutionFile = $"{baseSetup.ProjectPath}\\{baseSetup.ProjectName}.sln";
            var buildSolutionScript = $"& \"{GetMsBuildExePath()}\" \"{solutionFile}\" /t:Restore,Rebuild /p:RestorePackagesPath=\"C:\\Packs\" /p:Configuration=Release /p:Platform=x86";
            ExecutePowerShellScript(buildSolutionScript);

            return baseSetup;
        }

        private string GetVstestsConsoleExePath()
        {
            var possPath = $"{BaseGenAndBuildFixture.GetVsInstallRoot()}\\Common7\\IDE\\Extensions\\TestPlatform\\vstest.console.exe";

            if (File.Exists(possPath))
            {
                return possPath;
            }

            throw new FileNotFoundException("vstest.console.exe could not be found. If you installed Visual Studio somewhere other than the default location you will need to modify the test code.");
        }

        private string GetMsBuildExePath()
        {
            var possPath = $"{BaseGenAndBuildFixture.GetVsInstallRoot()}\\MSBuild\\Current\\Bin\\MSBuild.exe";

            if (File.Exists(possPath))
            {
                return possPath;
            }

            throw new FileNotFoundException("MSBuild.exe could not be found. If you installed Visual Studio somewhere other than the default location you will need to modify the test code.");
        }

        private void ReplaceInFiles(string find, string replace, string rootDirectory, string fileFilter)
        {
            foreach (var file in Directory.GetFiles(rootDirectory, fileFilter, SearchOption.AllDirectories))
            {
                // open, replace, overwrite
                var contents = File.ReadAllText(file);
                var newContent = contents.Replace(find, replace);
                File.WriteAllText(file, newContent);
            }
        }

        protected string InstallCertificate((string SolutionPath, string ProjectName) baseSetup)
        {
            var cerFile = $"{baseSetup.SolutionPath}\\{baseSetup.ProjectName}\\AppPackages\\{baseSetup.ProjectName}_1.0.0.0_x86_Test\\{baseSetup.ProjectName}_1.0.0.0_x86.cer";

            ExecutePowerShellScript($"& certutil.exe -addstore TrustedPeople \"{cerFile}\"");

            return cerFile;
        }

        protected void ChangeProjectToNotUseDotNetNativeToolchain((string SolutionPath, string ProjectName) baseSetup, string language)
        {
            var projFileName = $"{baseSetup.SolutionPath}\\{baseSetup.ProjectName}\\{baseSetup.ProjectName}.{GetProjectExtension(language)}";

            var projFileContents = File.ReadAllText(projFileName);

            var newProjFileContents = projFileContents.Replace(
                                                                "<UseDotNetNativeToolchain>true</UseDotNetNativeToolchain>",
                                                                "<UseDotNetNativeToolchain>false</UseDotNetNativeToolchain>");

            File.WriteAllText(projFileName, newProjFileContents, Encoding.UTF8);
        }

        private Collection<PSObject> ExecutePowerShellScript(string script, bool outputOnError = false)
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ps.AddScript(script);

                Collection<PSObject> psOutput = ps.Invoke();

                if (ps.Streams.Error.Count > 0)
                {
                    foreach (var errorRecord in ps.Streams.Error)
                    {
                        Debug.WriteLine(errorRecord.ToString());
                    }

                    // Some things (such as failing test execution) report an error but we still want the full output
                    if (!outputOnError)
                    {
                        throw new PSInvalidOperationException(ps.Streams.Error.First().ToString());
                    }
                }

                return psOutput;
            }
        }

        protected class VisualComparisonTestDetails
        {
            public string CertificatePath { get; set; }

            public string PackageFamilyName { get; set; }

            public string PackageFullName { get; set; }

            public string ProjectName { get; set; }
        }
    }
}
