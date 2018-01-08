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
        private readonly VisualComparisonTestsFixture _visualFixture;

        public VisualComparisonTests(GenerationFixture fixture)
        {
            _fixture = fixture;
            _visualFixture = new VisualComparisonTestsFixture();
        }

        // [Theory]
        // [MemberData("GetMultiLanguageProjectsAndFrameworks")]
        [Fact]
        [Trait("ExecutionSet", "ManualOnly")]
        [Trait("Type", "WinAppDriver")]
        public async Task EnsureLauchPageVisualsAreEquivalentAsync() // (string projectType, string framework)
        {
            var projectType = "Blank";
            var framework = "CodeBehind";

            var genIdentities = new[]
                {
                    "wts.Page.Chart",
                };

            var app1Details = await SetUpProjectForUiTestComparisonAsync(ProgrammingLanguages.CSharp, projectType, framework, genIdentities, lastPageIsHome: true);
            var app2Details = await SetUpProjectForUiTestComparisonAsync(ProgrammingLanguages.VisualBasic, projectType, framework, genIdentities, lastPageIsHome: true);

            // create test project
            /* create test project
             */

            UninstallAppx(app1Details.PackageFullName);
            UninstallAppx(app2Details.PackageFullName);

            RemoveCertificate(app1Details.CertificatePath);
            RemoveCertificate(app2Details.CertificatePath);

            // if all went well, delete project files
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

        private void UninstallAppx(string packageFullName)
        {
            ExecutePowerShellScript($"Remove-AppxPackage -Package {packageFullName}");
        }

        private async Task<VisualComparisonTestDetails> SetUpProjectForUiTestComparisonAsync(string language, string projectType, string framework, IEnumerable<string> genIdentities, bool lastPageIsHome = false)
        {
            var result = new VisualComparisonTestDetails();

            var baseSetup = await SetUpComparisonProjectAsync(language, projectType, framework, genIdentities, lastPageIsHome);

            ChangeProjectToNotUseDotNetNativeToolchain(baseSetup, language); // So building release version is fast

            ////Build solution in release mode  // Building in release mode creates the APPX and certificate files we need
            var solutionFile = $"{baseSetup.ProjectPath}.sln";
            var buildSolutionScript = $"\"C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\Enterprise\\MSBuild\\15.0\\Bin\\MSBuild.exe\" msbuild {solutionFile} /t:Build /p:Configuration=Release /p:Platform=x86";
            ExecutePowerShellScript(buildSolutionScript);

            result.CertificatePath = InstallCertificate(baseSetup);

            ////install appx
            var appxFile = $"{baseSetup.ProjectPath}\\AppPackages\\{baseSetup.ProjectName}_1.0.0.0_x86_Test\\{baseSetup.ProjectName}_1.0.0.0_x86.appx";
            ExecutePowerShellScript($"Add-AppxPackage -Path {appxFile}");

            // get app package name
            var manifest = XDocument.Load($"{baseSetup.ProjectPath}\\Package.appxmanifest");
            var xnm = new XmlNamespaceManager(new NameTable());
            xnm.AddNamespace(string.Empty, "http://schemas.microsoft.com/appx/manifest/foundation/windows10");
            var packageName = manifest.XPathSelectElement("Package/Identity[@name='Name']/@value", xnm);

            // get details from appx install
            var getPackageNamesScript = $"$PackageName = \"{packageName}\"" + @"
$packageDetails = Get-AppxPackage -Name $PackageName

$packageFamilyName = $packageDetails.PackageFamilyName
$packageFullName = $packageDetails.PackageFullName

Write-Host $packageFamilyName
Write-Host $packageFullName";

            var response = ExecutePowerShellScript(getPackageNamesScript);

            result.PackageFamilyName = response[0].ToString();
            result.PackageFullName = response[1].ToString();

            return result;
        }

        private string InstallCertificate((string ProjectPath, string ProjectName) baseSetup)
        {
            var cerFile = $"{baseSetup.ProjectPath}\\AppPackages\\{baseSetup.ProjectName}_1.0.0.0_x86_Test\\{baseSetup.ProjectName}_1.0.0.0_x86.cer";

            ExecutePowerShellScript($"certutil.exe -addstore TrustedPeople {cerFile}");

            return cerFile;
        }

        private void ChangeProjectToNotUseDotNetNativeToolchain((string ProjectPath, string ProjectName) baseSetup, string language)
        {
            var projFileName = $"{baseSetup.ProjectPath}/{baseSetup.ProjectName}.{GetProjectExtension(language)}";

            var projFileContents = File.ReadAllText(projFileName);

            var newProjFileContents = projFileContents.Replace(
                                                                "<UseDotNetNativeToolchain>true</UseDotNetNativeToolchain>",
                                                                "<UseDotNetNativeToolchain>false</UseDotNetNativeToolchain>");

            File.WriteAllText(projFileName, newProjFileContents, Encoding.UTF8);
        }

        private Collection<PSObject> ExecutePowerShellScript(string script)
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

                    throw new PSInvalidOperationException(ps.Streams.Error.First().ToString());
                }

                return psOutput;
            }
        }

        private class VisualComparisonTestDetails
        {
            public string CertificatePath { get; set; }

            public string PackageFamilyName { get; set; }

            public string PackageFullName { get; set; }
        }
    }
}
