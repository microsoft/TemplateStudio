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
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.Templates.Core;
using Xunit;

namespace Microsoft.Templates.Test
{
    [Collection("GenerationCollection")]
    public class VisualComparisonTests : BaseGenAndBuildTests
    {
        public VisualComparisonTests(GenerationFixture fixture)
        {
            _fixture = fixture;
            _fixture.InitializeFixtureAsync(this);
        }

        public static IEnumerable<object[]> GetAllSinglePageApps()
        {
            foreach (var projectType in new[] { "Blank", "SplitView", "TabbedPivot" })
            {
                foreach (var framework in new[] { "CodeBehind", "MVVMBasic", "MVVMLight" })
                {
                    // For other pages see https://github.com/Microsoft/WindowsTemplateStudio/issues/1717
                    var pagesThatSupportUiTesting = new[]
                    {
                        "wts.Page.Blank",
                        "wts.Page.Chart",
                        "wts.Page.ImageGallery",
                        "wts.Page.MasterDetail",
                        "wts.Page.TabbedPivot",
                    };

                    foreach (var page in pagesThatSupportUiTesting)
                    {
                        yield return new object[] { projectType, framework, page };
                    }
                }
            }
        }

        // Note. Visual Studio MUST be running as Admin to run this test.
        [Theory]
        [MemberData("GetAllSinglePageApps")]
        [Trait("ExecutionSet", "ManualOnly")]
        [Trait("Type", "WinAppDriver")]
        public async Task EnsureLaunchPageVisualsAreEquivalentAsync(string projectType, string framework, string page)
        {
            var genIdentities = new[] { page };

            CheckWinAppDriverInstalled();

            var app1Details = await SetUpProjectForUiTestComparisonAsync(ProgrammingLanguages.CSharp, projectType, framework, genIdentities, lastPageIsHome: true);
            var app2Details = await SetUpProjectForUiTestComparisonAsync(ProgrammingLanguages.VisualBasic, projectType, framework, genIdentities, lastPageIsHome: true);

            var testProjectDetails = SetUpTestProject(app1Details, app2Details);

#pragma warning disable SA1008 // Opening parenthesis must be spaced correctly - StyleCop can't handle Tuples
            var (testSuccess, testOutput) = RunWinAppDriverTests(testProjectDetails);
#pragma warning restore SA1008 // Opening parenthesis must be spaced correctly

            // Note that failing tests will leave the projects behind, plus the apps and test certificates installed
            if (testSuccess)
            {
                UninstallAppx(app1Details.PackageFullName);
                UninstallAppx(app2Details.PackageFullName);

                RemoveCertificate(app1Details.CertificatePath);
                RemoveCertificate(app2Details.CertificatePath);

                // Parent of images folder also contains the test project
                Fs.SafeDeleteDirectory(Path.Combine(testProjectDetails.imagesFolder, ".."));
            }

            var outputMessages = string.Join(Environment.NewLine, testOutput);

            if (Directory.GetFiles(testProjectDetails.imagesFolder, "*.*-Diff.png").Any())
            {
                Assert.True(
                    testSuccess,
                    $"Failing test images in {testProjectDetails.imagesFolder}{Environment.NewLine}{Environment.NewLine}{outputMessages}");
            }
            else
            {
                Assert.True(testSuccess, outputMessages);
            }
        }

        private void CheckWinAppDriverInstalled()
        {
            Assert.True(
                File.Exists(@"C:\Program Files (x86)\Windows Application Driver\WinAppDriver.exe"),
                "WinAppDriver is not installed. Download from https://github.com/Microsoft/WinAppDriver/releases");
        }

#pragma warning disable SA1008 // Opening parenthesis must be spaced correctly - StyleCop can't handle Tuples
        private (bool Success, List<string> TextOutput) RunWinAppDriverTests((string projectFolder, string imagesFolder) testProjectDetails)
#pragma warning restore SA1008 // Opening parenthesis must be spaced correctly
        {
            var result = false;

            StartWinAppDriverIfNotRunning();

            var buildOutput = Path.Combine(testProjectDetails.projectFolder, "bin", "Debug", "AutomatedUITests.dll");
            var runTestsScript = $"& \"C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\Enterprise\\Common7\\IDE\\Extensions\\TestPlatform\\vstest.console.exe\" \"{buildOutput}\" ";

            // Test failures will be treated as an error but they are handled below
            var output = ExecutePowerShellScript(runTestsScript, outputOnError: true);

            var outputText = new List<string>();

            foreach (var outputLine in output)
            {
                var outputLineString = outputLine.ToString();

                outputText.Add(outputLineString);

                if (outputLineString.StartsWith("Total tests: ") && outputLineString.Contains("Failed: 0."))
                {
                    result = true;
                }
            }

            StopWinAppDriverIfRunning();

            return (result, outputText);
        }

#pragma warning disable SA1008 // Opening parenthesis must be spaced correctly - StyleCop can't handle Tuples
        private (string projectFolder, string imagesFolder) SetUpTestProject(VisualComparisonTestDetails app1Details, VisualComparisonTestDetails app2Details)
#pragma warning restore SA1008 // Opening parenthesis must be spaced correctly
        {
            var rootFolder = $"{Path.GetPathRoot(Environment.CurrentDirectory)}UIT\\VIS\\{DateTime.Now:dd_HHmmss}\\";
            var projectFolder = Path.Combine(rootFolder, "TestProject");
            var imagesFolder = Path.Combine(rootFolder, "Images");

            Fs.EnsureFolder(rootFolder);
            Fs.EnsureFolder(projectFolder);
            Fs.EnsureFolder(imagesFolder);

            // Copy base project
            Fs.CopyRecursive(@"..\..\VisualTests\TestProjectSource", projectFolder, overwrite: true);

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
                .Replace("***FOLDER-GOES-HERE***", imagesFolder);

            File.WriteAllText(appInfoFileName, newAppInfoFileContents, Encoding.UTF8);

            // build test project
            var restoreNugetScript = $"& \"{projectFolder}\\nuget.exe\" restore \"{projectFolder}\\AutomatedUITests.csproj\" -PackagesDirectory \"{projectFolder}\\Packages\"";
            ExecutePowerShellScript(restoreNugetScript);
            var buildSolutionScript = $"& \"C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\Enterprise\\MSBuild\\15.0\\Bin\\MSBuild.exe\" \"{projectFolder}\\AutomatedUITests.sln\" /t:Rebuild /p:RestorePackagesPath=\"{projectFolder}\\Packages\" /p:Configuration=Debug /p:Platform=\"Any CPU\"";
            ExecutePowerShellScript(buildSolutionScript);

            return (projectFolder, imagesFolder);
        }

        private void RemoveCertificate(string certificatePath)
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

        private void StartWinAppDriverIfNotRunning()
        {
            var script = @"
$wad = Get-Process WinAppDriver -ErrorAction SilentlyContinue

if ($wad -eq $Null)
{
    Start-Process ""C:\Program Files (x86)\Windows Application Driver\WinAppDriver.exe""
}";

            ExecutePowerShellScript(script);
        }

        private void StopWinAppDriverIfRunning()
        {
            var script = @"
$wad = Get-Process WinAppDriver -ErrorAction SilentlyContinue

if ($wad -ne $null)
{
    $wad.CloseMainWindow()
}";

            ExecutePowerShellScript(script);
        }

        private void UninstallAppx(string packageFullName)
        {
            ExecutePowerShellScript($"Remove-AppxPackage -Package {packageFullName}");
        }

        private async Task<VisualComparisonTestDetails> SetUpProjectForUiTestComparisonAsync(string language, string projectType, string framework, IEnumerable<string> genIdentities, bool lastPageIsHome = false)
        {
            var result = new VisualComparisonTestDetails();

            var baseSetup = await SetUpComparisonProjectAsync(language, projectType, framework, genIdentities, lastPageIsHome);

            result.ProjectName = baseSetup.ProjectName;

            ChangeProjectToNotUseDotNetNativeToolchain(baseSetup, language); // So building release version is fast

            ////Build solution in release mode  // Building in release mode creates the APPX and certificate files we need
            var solutionFile = $"{baseSetup.ProjectPath}\\{baseSetup.ProjectName}.sln";
            var buildSolutionScript = $"& \"C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\Enterprise\\MSBuild\\15.0\\Bin\\MSBuild.exe\" \"{solutionFile}\" /t:Restore,Rebuild /p:RestorePackagesPath=\"C:\\Packs\" /p:Configuration=Release /p:Platform=x86";
            ExecutePowerShellScript(buildSolutionScript);

            result.CertificatePath = InstallCertificate(baseSetup);

            ////install appx
            var appxFile = $"{baseSetup.ProjectPath}\\{baseSetup.ProjectName}\\AppPackages\\{baseSetup.ProjectName}_1.0.0.0_x86_Test\\{baseSetup.ProjectName}_1.0.0.0_x86.appx";
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

        private string InstallCertificate((string SolutionPath, string ProjectName) baseSetup)
        {
            var cerFile = $"{baseSetup.SolutionPath}\\{baseSetup.ProjectName}\\AppPackages\\{baseSetup.ProjectName}_1.0.0.0_x86_Test\\{baseSetup.ProjectName}_1.0.0.0_x86.cer";

            ExecutePowerShellScript($"& certutil.exe -addstore TrustedPeople \"{cerFile}\"");

            return cerFile;
        }

        private void ChangeProjectToNotUseDotNetNativeToolchain((string SolutionPath, string ProjectName) baseSetup, string language)
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

        private class VisualComparisonTestDetails
        {
            public string CertificatePath { get; set; }

            public string PackageFamilyName { get; set; }

            public string PackageFullName { get; set; }

            public string ProjectName { get; set; }
        }
    }
}
