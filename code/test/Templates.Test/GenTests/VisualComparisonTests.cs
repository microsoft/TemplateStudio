// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
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
        }

        //[Theory]
        // [MemberData("GetMultiLanguageProjectsAndFrameworks")]
        [Fact]
        [Trait("ExecutionSet", "ManualOnly")]
        [Trait("Type", "WinAppDriver")]
        public async Task EnsureProjectsGeneratedWithDifferentLanguagesAreEquivalentAsync() //(string projectType, string framework)
        {
            var projectType = "Blank";
            var framework = "CodeBehind";

            var genIdentities = new[]
                {
                    "wts.Page.Chart",
                };

#pragma warning disable SA1008 // Opening parenthesis must be spaced correctly - C#7.0 feature StyleCop can't handle
            var (csResultPath, csProjectName) = await SetUpComparisonProjectAsync(ProgrammingLanguages.CSharp, projectType, framework, genIdentities, lastPageIsHome: true);
            var (vbResultPath, vbProjectName) = await SetUpComparisonProjectAsync(ProgrammingLanguages.VisualBasic, projectType, framework, genIdentities, lastPageIsHome: true);
#pragma warning restore SA1008 // Opening parenthesis must be spaced correctly

        }
    }
}
