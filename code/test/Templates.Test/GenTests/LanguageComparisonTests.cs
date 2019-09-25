// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Helpers;
using Microsoft.Templates.Fakes;
using Xunit;

namespace Microsoft.Templates.Test
{
    [Collection("GenerationCollection")]
    public class LanguageComparisonTests : BaseGenAndBuildTests
    {
        public LanguageComparisonTests(GenerationFixture fixture)
            : base(fixture)
        {
        }

        // This test is manual only as it will fail when C# templates are updated but their VB equivalents haven't been.
        // The VB versions should have equivalent changes made also but we don't want the CI to fail when just the VB changes are made.
        [Theory]
        [MemberData(nameof(GetMultiLanguageProjectsAndFrameworks))]
        [Trait("ExecutionSet", "ManualOnly")]
        [Trait("Type", "GenerationLanguageComparison")]
        public async Task EnsureProjectsGeneratedWithDifferentLanguagesAreEquivalentForcedLoginAsync(string projectType, string framework)
        {
            await EnsureProjectsGeneratedWithDifferentLanguagesAreEquivalentAsync(projectType, framework, "wts.Service.IdentityForcedLogin");
        }

        // This test is manual only as it will fail when C# templates are updated but their VB equivalents haven't been.
        // The VB versions should have equivalent changes made also but we don't want the CI to fail when just the VB changes are made.
        [Theory]
        [MemberData(nameof(GetMultiLanguageProjectsAndFrameworks))]
        [Trait("ExecutionSet", "ManualOnly")]
        [Trait("Type", "GenerationLanguageComparison")]
        public async Task EnsureProjectsGeneratedWithDifferentLanguagesAreEquivalentOptionalLoginAsync(string projectType, string framework)
        {
            await EnsureProjectsGeneratedWithDifferentLanguagesAreEquivalentAsync(projectType, framework, "wts.Service.IdentityOptionalLogin");
        }

        private async Task EnsureProjectsGeneratedWithDifferentLanguagesAreEquivalentAsync(string projectType, string framework, string extraIdentity)
        {
            var genIdentities = GetPagesAndFeaturesForMultiLanguageProjects().ToList();

            genIdentities.Add(extraIdentity);

            var (csResultPath, csProjectName) = await SetUpComparisonProjectAsync(ProgrammingLanguages.CSharp, projectType, framework, genIdentities);
            var (vbResultPath, vbProjectName) = await SetUpComparisonProjectAsync(ProgrammingLanguages.VisualBasic, projectType, framework, genIdentities);

            EnsureAllEquivalentFileNamesAreUsed(csResultPath, vbResultPath);
            EnsureResourceStringsAreIdenticalAndAllUsed(csResultPath, csProjectName, vbResultPath, vbProjectName);
            EnsureContentsOfAssetsFolderIsIdentical(csResultPath, csProjectName, vbResultPath, vbProjectName);
            EnsureContentsOfStylesFolderIsIdentical(csResultPath, csProjectName, vbResultPath, vbProjectName);
            EnsureFileCommentsAreIdentical(vbResultPath);
            EnsureCodeFilesContainIdenticalElements(vbResultPath);
            EnsureEquivalentErrorHandling(vbResultPath);

            Fs.SafeDeleteDirectory(csResultPath);
            Fs.SafeDeleteDirectory(vbResultPath);
        }

        private static void EnsureAllEquivalentFileNamesAreUsed(string csResultPath, string vbResultPath)
        {
            var allCsFiles = new DirectoryInfo(csResultPath).GetFiles("*.*", SearchOption.AllDirectories).Select(f => f.FullName).ToList();
            var allVbFiles = new DirectoryInfo(vbResultPath).GetFiles("*.*", SearchOption.AllDirectories).Select(f => f.FullName).ToList();
            var equalNumberOfFiles = allCsFiles.Count == allVbFiles.Count;
            Assert.True(equalNumberOfFiles, $"Differing number of files in the generated projects ({csResultPath.Replace("CS", " * ")}).");

            for (var i = 0; i < allVbFiles.Count; i++)
            {
                var fileNameMatches = allCsFiles.Contains(VbFileToCsEquivalent(allVbFiles[i]));
                Assert.True(fileNameMatches, $"File '{allVbFiles[i]}' does not have a C# equivalent.");
            }
        }

        private static void EnsureResourceStringsAreIdenticalAndAllUsed(string csResultPath, string csProjectName, string vbResultPath, string vbProjectName)
        {
            var csReswFilePath = Path.Combine(csResultPath, csProjectName, "Strings", "en-us", "Resources.resw");
            var vbReswFilePath = Path.Combine(vbResultPath, vbProjectName, "Strings", "en-us", "Resources.resw");

            var csResourcesString = File.ReadAllText(csReswFilePath);
            var vbResourcesString = File.ReadAllText(vbReswFilePath);

            // Account for different project names in resources files
            vbResourcesString = vbResourcesString.Replace(vbProjectName, csProjectName);

            var reswFilesMatch = csResourcesString == vbResourcesString;
            Assert.True(reswFilesMatch, $"Resource files do not match ({csResultPath.Replace("CS", "*")}).");

            // Ensure all resources are used
            var resourceKeys = new List<string>();
            var resourceXml = new XmlDocument();
            resourceXml.LoadXml(csResourcesString);

            var nodes = resourceXml.GetElementsByTagName("data");
            for (int i = 0; i < nodes.Count; i++)
            {
                // Assume resources containing dots are Uids and so ignore them
                var resourceName = nodes[i].Attributes.GetNamedItem("name").Value;
                if (!resourceName.Contains("."))
                {
                    resourceKeys.Add(resourceName);
                }
            }

            foreach (var filter in new[] { "*.vb", "Package.appxmanifest" })
            {
                foreach (var vbFile in new DirectoryInfo(Path.Combine(vbResultPath, vbProjectName)).GetFiles(filter, SearchOption.AllDirectories))
                {
                    var vbFileContents = File.ReadAllText(vbFile.FullName);

                    for (var i = resourceKeys.Count - 1; i >= 0; i--)
                    {
                        if (vbFileContents.Contains(resourceKeys[i]))
                        {
                            resourceKeys.RemoveAt(i);
                        }
                    }

                    if (!resourceKeys.Any())
                    {
                        break;
                    }
                }
            }

            Assert.True(!resourceKeys.Any(), $"Resource strings are defined but not used in VB files: {string.Join("; ", resourceKeys)}");
        }

        private static void EnsureContentsOfAssetsFolderIsIdentical(string csResultPath, string csProjectName, string vbResultPath, string vbProjectName)
        {
            var csAssets = new DirectoryInfo(Path.Combine(csResultPath, csProjectName, "Assets")).GetFiles().OrderBy(f => f.FullName).ToList();
            var vbAssets = new DirectoryInfo(Path.Combine(vbResultPath, vbProjectName, "Assets")).GetFiles().OrderBy(f => f.FullName).ToList();

            for (var i = 0; i < csAssets.Count; i++)
            {
                var styleFileMatches = BinaryFileEquals(csAssets[i].FullName, vbAssets[i].FullName);
                Assert.True(styleFileMatches, $"Asset file '{csAssets[i].Name}' does not match ({csResultPath.Replace("CS", "*")}).");
            }
        }

        private static void EnsureContentsOfStylesFolderIsIdentical(string csResultPath, string csProjectName, string vbResultPath, string vbProjectName)
        {
            var csStyles = new DirectoryInfo(Path.Combine(csResultPath, csProjectName, "Styles")).GetFiles().OrderBy(f => f.FullName).ToList();
            var vbStyles = new DirectoryInfo(Path.Combine(vbResultPath, vbProjectName, "Styles")).GetFiles().OrderBy(f => f.FullName).ToList();

            for (var i = 0; i < csStyles.Count; i++)
            {
                var styleFileMatches = File.ReadAllText(csStyles[i].FullName) == File.ReadAllText(vbStyles[i].FullName);
                Assert.True(styleFileMatches, $"Style file '{csStyles[i].Name}' does not match ({csResultPath.Replace("CS", "*")}).");
            }
        }

        private void EnsureFileCommentsAreIdentical(string vbResultPath)
        {
            var allVbFiles = new DirectoryInfo(vbResultPath).GetFiles("*.vb", SearchOption.AllDirectories).ToList();

            var failures = new List<string>();

            for (var i = 0; i < allVbFiles.Count; i++)
            {
                if (allVbFiles[i].Name.Contains("CommandLineActivationHandler.vb"))
                {
                    // This file contains code samples in comments that have different numbers of lines
                    continue;
                }

                var vbLines = File.ReadAllLines(allVbFiles[i].FullName);
                var vbCommentLines = vbLines.Where(l => l.TrimStart().StartsWith("'", StringComparison.Ordinal)).ToArray();
                var csLines = File.ReadAllLines(VbFileToCsEquivalent(allVbFiles[i].FullName));
                var csCommentLines = csLines.Where(l => l.TrimStart().StartsWith("/", StringComparison.Ordinal)).ToArray();

                if (vbCommentLines.Length != csCommentLines.Length)
                {
                    // Exception for comment which contains a sample code method.
                    if (allVbFiles[i].Name == "Program.vb" && ((vbCommentLines.Length + 1) == csCommentLines.Length))
                    {
                        // Do not report this as difference is expected
                    }
                    else
                    {
                        failures.Add(
                            $"File '{allVbFiles[i].FullName}' does not have the same number of comments as its C# equivalent. C# version has {csCommentLines.Length} while VB version has {vbCommentLines.Length}.");
                    }

                    continue;
                }

                var filesWithCommentsInDifferentOrder = new[] { "ToastNotificationsService.Samples.vb", "IdentityService.vb" };

                if (filesWithCommentsInDifferentOrder.Contains(allVbFiles[i].Name))
                {
                    for (int j = 0; j < vbCommentLines.Count(); j++)
                    {
                        var vbComment = vbCommentLines[j].TrimStart(' ', '\'');

                        if (!csCommentLines.Any(l => l.Contains(vbComment)))
                        {
                            failures.Add($"File '{allVbFiles[i].FullName}' does not have comments matching its C# equivalent.");
                            break;
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < vbCommentLines.Count(); j++)
                    {
                        var vbComment = vbCommentLines[j].TrimStart(' ', '\'').Replace(".vb.md", ".md"); // Allow for language specific doc files
                        var csComment = csCommentLines[j].TrimStart(' ', '/');

                        var commentsMatch = CommentsMatchOrAreEquivalent(vbComment, csComment);

                        if (!commentsMatch)
                        {
                            failures.Add($"File '{allVbFiles[i].FullName}' does not have comments matching its C# equivalent.");
                            break;
                        }
                    }
                }
            }

            Assert.True(!failures.Any(), string.Join(Environment.NewLine, failures));
        }

        private static bool CommentsMatchOrAreEquivalent(string vbComment, string csComment)
        {
            if (vbComment == csComment)
            {
                return true;
            }
            else
            {
                var codeCommentExceptions = new Dictionary<string, string>
                {
                    {
                        "Dim secondaryTileArguments = args.Arguments",
                        "var secondaryTileArguments = args.Arguments;"
                    },
                    {
                        "Dim tileUpdatesArguments = args.TileActivatedInfo.RecentlyShownNotifications",
                        "var tileUpdatesArguments = args.TileActivatedInfo.RecentlyShownNotifications;"
                    },
                    {
                        "This module holds sample data used by some generated pages to show how they can be used.",
                        "This class holds sample data used by some generated pages to show how they can be used."
                    },
                    {
                        "Await Singleton(Of HubNotificationsService).Instance.InitializeAsync()",
                        "await Singleton<HubNotificationsService>.Instance.InitializeAsync();"
                    },
                    {
                        "mapControl.MapServiceToken = String.Empty",
                        "mapControl.MapServiceToken = string.Empty;"
                    },
                    {
                        "map.MapServiceToken = String.Empty",
                        "map.MapServiceToken = string.Empty;"
                    },
                    {
                        "NavHelper.SetNavigateTo(navigationViewItem, GetType(MainPage))",
                        "NavHelper.SetNavigateTo(navigationViewItem, typeof(MainPage));"
                    },
                    {
                        "NavHelper.SetNavigateTo(navigationViewItem, GetType(MainViewModel).FullName)",
                        "NavHelper.SetNavigateTo(navigationViewItem, typeof(MainViewModel).FullName);"
                    },
                    {
                        "Await Singleton(Of HubNotificationsService).Instance.InitializeAsync().ConfigureAwait(False)",
                        "await Singleton<HubNotificationsService>.Instance.InitializeAsync().ConfigureAwait(false);"
                    }
                };

                return codeCommentExceptions.ContainsKey(vbComment) && codeCommentExceptions[vbComment] == csComment;
            }
        }

        private void EnsureCodeFilesContainIdenticalElements(string vbResultPath)
        {
            var failures = new List<string>();

            var allVbFiles = new DirectoryInfo(vbResultPath).GetFiles("*.vb", SearchOption.AllDirectories).Select(f => f.FullName).ToList();

            foreach (var vbFile in allVbFiles)
            {
                var csFile = VbFileToCsEquivalent(vbFile);

                var csCode = new StreamReader(csFile).ReadToEnd();
                var csTree = Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree.ParseText(csCode);
                var csRoot = (Microsoft.CodeAnalysis.CSharp.Syntax.CompilationUnitSyntax)csTree.GetRoot();
                var csProperties = csRoot.DescendantNodes().OfType<Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax>().Select(p => p.Identifier.Text + "-" + p.Type.ToString().ToLowerInvariant()).ToList();
                var csMethods = csRoot.DescendantNodes().OfType<Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax>().Select(m => m.Identifier.Text).ToList();
                var csEvents = csRoot.DescendantNodes().OfType<Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax>().Select(e => e.Declaration.Variables.First().Identifier.Text).ToList();
                csEvents.AddRange(csRoot.DescendantNodes().OfType<Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax>().Select(e => e.Identifier.Text).ToList());
                var csEnums = csRoot.DescendantNodes().OfType<Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax>().Select(e => e).ToList();
                var csEnumItems = (from csEnum in csEnums from csEnumMember in csEnum.Members select csEnum.Identifier.Text + csEnumMember.Identifier.Text).ToList();

                var csConstants = new List<string>();

                foreach (var field in csRoot.DescendantNodes().OfType<Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax>())
                {
                    foreach (var modifier in field.Modifiers)
                    {
                        if (modifier.ValueText.Equals("const"))
                        {
                            var constName = field.Declaration.Variables[0].Identifier.ToString();
                            var constValue = field.Declaration.Variables[0].Initializer.Value.ToString();
                            csConstants.Add($"{constName}={constValue}");
                        }
                    }
                }

                var vbCode = new StreamReader(vbFile).ReadToEnd();
                var vbTree = VisualBasicSyntaxTree.ParseText(vbCode);
                var vbRoot = (CompilationUnitSyntax)vbTree.GetRoot();
                var vbProperties = GetVBPropertiesForComparison(vbRoot);
                var vbMethods = vbRoot.DescendantNodes().OfType<MethodStatementSyntax>().Select(m => m.Identifier.Text.Replace("[", string.Empty).Replace("]", string.Empty)).ToList();
                var vbEvents = vbRoot.DescendantNodes().OfType<EventStatementSyntax>().Select(e => e.Identifier.Text).ToList();
                var vbEnumItems = vbRoot.DescendantNodes().OfType<EnumMemberDeclarationSyntax>().Select(e => (e.Parent as EnumBlockSyntax).EnumStatement.Identifier.ToString() + e.Identifier).ToList();

                var vbConstants = new List<string>();

                foreach (var field in vbRoot.DescendantNodes().OfType<FieldDeclarationSyntax>())
                {
                    foreach (var modifier in field.Modifiers)
                    {
                        if (modifier.ValueText.Equals("Const"))
                        {
                            var constName = field.Declarators[0].Names[0].ToString();
                            var constValue = field.Declarators[0].Initializer.Value.ToString();

                            // A Single in VB would be like '0.1F' and be compared with a float in C# which would be like '0.1f'
                            if (field.Declarators.First().AsClause.Type().ToString() == "Single")
                            {
                                constValue = constValue.ToLowerInvariant();
                            }

                            vbConstants.Add($"{constName}={constValue}");
                        }
                    }
                }

                foreach (var csProp in csProperties)
                {
                    if (vbProperties.Contains(csProp))
                    {
                        vbProperties.Remove(csProp);
                    }
                    else
                    {
                        failures.Add($"'{csFile}' includes property '{csProp}' which isn't in the VB equivalent.");
                    }
                }

                failures.AddRange(vbProperties.Select(vbProp => $"'{vbFile}' includes property '{vbProp}' which isn't in the C# equivalent."));

                foreach (var csMethod in csMethods)
                {
                    if (vbMethods.Contains(csMethod))
                    {
                        vbMethods.Remove(csMethod);
                    }
                    else
                    {
                        failures.Add($"'{csFile}' includes method '{csMethod}' which isn't in the VB equivalent.");
                    }
                }

                failures.AddRange(vbMethods.Where(m => m != "InlineAssignHelper").Select(vbMethod => $"'{vbFile}' includes method '{vbMethod}' which isn't in the C# equivalent."));

                foreach (var csEvent in csEvents)
                {
                    if (vbEvents.Contains(csEvent))
                    {
                        vbEvents.Remove(csEvent);
                    }
                    else
                    {
                        failures.Add($"'{csFile}' includes event '{csEvent}' which isn't in the VB equivalent.");
                    }
                }

                failures.AddRange(vbEvents.Select(vbEvent => $"'{vbFile}' includes event '{vbEvent}' which isn't in the C# equivalent."));

                foreach (var csEnum in csEnumItems)
                {
                    if (vbEnumItems.Contains(csEnum))
                    {
                        vbEnumItems.Remove(csEnum);
                    }
                    else
                    {
                        failures.Add($"'{csFile}' includes enum '{csEnum}' which isn't in the VB equivalent.");
                    }
                }

                failures.AddRange(vbEnumItems.Select(vbEnum => $"'{vbFile}' includes enum '{vbEnum}' which isn't in the C# equivalent."));

                foreach (var csConst in csConstants)
                {
                    if (vbConstants.Contains(csConst))
                    {
                        vbConstants.Remove(csConst);
                    }
                    else
                    {
                        failures.Add($"'{csFile}' includes constant '{csConst}' which isn't in the VB equivalent.");
                    }
                }

                failures.AddRange(vbConstants.Select(vbConst => $"'{vbFile}' includes constant '{vbConst}' which isn't in the C# equivalent."));
            }

            Assert.True(!failures.Any(), string.Join(Environment.NewLine, failures));
        }

        private void EnsureEquivalentErrorHandling(string vbResultPath)
        {
            var failures = new List<string>();

            var allVbFiles = new DirectoryInfo(vbResultPath).GetFiles("*.vb", SearchOption.AllDirectories).Select(f => f.FullName).ToList();

            foreach (var vbFile in allVbFiles)
            {
                var csFile = VbFileToCsEquivalent(vbFile);

                var csCode = new StreamReader(csFile).ReadToEnd();
                var csTree = Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree.ParseText(csCode);
                var csRoot = (Microsoft.CodeAnalysis.CSharp.Syntax.CompilationUnitSyntax)csTree.GetRoot();
                var csExceptions = csRoot.DescendantNodes().OfType<Microsoft.CodeAnalysis.CSharp.Syntax.CatchClauseSyntax>().Select(p => p.Declaration.Type.ToString()).ToList();

                var vbCode = new StreamReader(vbFile).ReadToEnd();
                var vbTree = VisualBasicSyntaxTree.ParseText(vbCode);
                var vbRoot = (CompilationUnitSyntax)vbTree.GetRoot();
                var vbExceptions = vbRoot.DescendantNodes().OfType<CatchStatementSyntax>().Select(m => m.AsClause.Type.ToString()).ToList();

                foreach (var csException in csExceptions)
                {
                    if (vbExceptions.Contains(csException))
                    {
                        vbExceptions.Remove(csException);
                    }
                    else
                    {
                        failures.Add($"'{csFile}' includes catch for '{csException}' which isn't in the VB equivalent.");
                    }
                }

                failures.AddRange(vbExceptions.Where(m => m != "InlineAssignHelper").Select(vbExc => $"'{vbFile}' includes catch for  '{vbExc}' which isn't in the C# equivalent."));
            }

            Assert.True(!failures.Any(), string.Join(Environment.NewLine, failures));
        }

        private List<string> GetVBPropertiesForComparison(CompilationUnitSyntax vbRoot)
        {
            List<SyntaxNode> syntaxNodes = vbRoot.DescendantNodes().OfType<PropertyStatementSyntax>().Select(p => p as SyntaxNode).ToList();

            syntaxNodes.AddRange(vbRoot.DescendantNodes().OfType<PropertyBlockSyntax>().ToList());

            var result = new List<string>(syntaxNodes.Count);

            foreach (var syntaxNode in syntaxNodes)
            {
                var identifier = syntaxNode.ChildTokens().FirstOrDefault(t => t.RawKind == (int)SyntaxKind.IdentifierToken).ValueText;

                if (identifier == null && syntaxNode is PropertyBlockSyntax)
                {
                    identifier = (syntaxNode as PropertyBlockSyntax).PropertyStatement.Identifier.ValueText;
                }

                var descendentNodes = syntaxNode.DescendantNodes().ToList();

                string propertyType;

                if (descendentNodes.Any(n => n is SimpleAsClauseSyntax))
                {
                    propertyType = descendentNodes.OfType<SimpleAsClauseSyntax>().FirstOrDefault()?.Type.ToString();
                }
                else if (descendentNodes.Any(n => n is GenericNameSyntax))
                {
                    propertyType = descendentNodes.OfType<GenericNameSyntax>().FirstOrDefault()?.ToString();
                }
                else if (descendentNodes.Any(n => n is PredefinedTypeSyntax))
                {
                    propertyType = descendentNodes.OfType<PredefinedTypeSyntax>().FirstOrDefault()?.Keyword.ValueText;
                }
                else if (descendentNodes.Any(n => n is ObjectCreationExpressionSyntax))
                {
                    propertyType = descendentNodes.OfType<ObjectCreationExpressionSyntax>().FirstOrDefault()?.Type.ToString();
                }
                else
                {
                    throw new ArgumentException($"Unknown type for property {identifier}");
                }

                var csEquivType = GetCSharpEquivalentProperty(propertyType);

                result.Add($"{identifier}-{csEquivType}");
            }

            return result.Distinct().ToList();
        }

        private object GetCSharpEquivalentProperty(string propertyType)
        {
            return propertyType.ToLowerInvariant()
                               .Replace("boolean", "bool")
                               .Replace("integer", "int")
                               .Replace("(of ", "<")
                               .Replace(")", ">");
        }

        private static string VbFileToCsEquivalent(string vbFilePath)
        {
            return vbFilePath.Replace("VB", "CS")
                .Replace(".vb", ".cs")
                .Replace("My Project", "Properties");
        }

        private static bool BinaryFileEquals(string fileName1, string fileName2)
        {
            using (var file1 = new FileStream(fileName1, FileMode.Open))
            using (var file2 = new FileStream(fileName2, FileMode.Open))
            {
                return FileStreamEquals(file1, file2);
            }
        }

        private static bool FileStreamEquals(Stream stream1, Stream stream2)
        {
            const int bufferSize = 2048;
            var buffer1 = new byte[bufferSize];
            var buffer2 = new byte[bufferSize];

            while (true)
            {
                var count1 = stream1.Read(buffer1, 0, bufferSize);
                var count2 = stream2.Read(buffer2, 0, bufferSize);

                if (count1 != count2)
                {
                    return false;
                }

                if (count1 == 0)
                {
                    return true;
                }

                if (!buffer1.Take(count1).SequenceEqual(buffer2.Take(count2)))
                {
                    return false;
                }
            }
        }
    }
}
