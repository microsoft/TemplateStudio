// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Xunit;

namespace Microsoft.Templates.Core.Test.PostActions.Catalog
{
    [Trait("ExecutionSet", "Minimum")]
    public class MergeResourceDictionaryPostactionTest : IContextProvider
    {
        public string ProjectName => "TestResourceDictionaryPostAction";

        public string OutputPath { get; set; }

        public string ProjectPath => Directory.GetCurrentDirectory();

        public string SolutionPath => string.Empty;

        public string TempGenerationPath => string.Empty;

        public List<string> ProjectItems => new List<string>();

        public List<string> FilesToOpen => new List<string>();

        public List<FailedMergePostAction> FailedMergePostActions => new List<FailedMergePostAction>();

        public Dictionary<string, List<MergeInfo>> MergeFilesFromProject => new Dictionary<string, List<MergeInfo>>();

        public Dictionary<ProjectMetricsEnum, double> ProjectMetrics => new Dictionary<ProjectMetricsEnum, double>();

        [Fact]
        public void MergeResourceDictionaryPostaction()
        {
            var source = Path.GetFullPath(@".\TestData\Merge\Style.xaml");
            var postaction = Path.GetFullPath(@".\TestData\Merge\Style_postaction.xaml");
            var expected = File.ReadAllText(@".\TestData\Merge\Style_expected.xaml").Replace("\r\n", string.Empty).Replace("\n", string.Empty);

            var config = new MergeConfiguration(postaction, true);

            var mergeResourceDictionaryPostAction = new MergeResourceDictionaryPostAction(config);
            mergeResourceDictionaryPostAction.Execute();

            var result = File.ReadAllText(source).Replace("\r\n", string.Empty).Replace("\n", string.Empty);

            Assert.Equal(result, expected);
        }

        [Fact]
        public void MergeResourceDictionaryPostaction_Failing()
        {
            var source = Path.GetFullPath(@".\TestData\Merge\Style_fail.xaml");
            var postaction = Path.GetFullPath(@".\TestData\Merge\Style_fail_postaction.xaml");
            var expected = File.ReadAllText(@".\TestData\Merge\Style_expected.xaml");

            GenContext.Current = this;
            var config = new MergeConfiguration(postaction, true);

            var mergeResourceDictionaryPostAction = new MergeResourceDictionaryPostAction(config);

            Exception ex = Assert.Throws<InvalidDataException>(() => mergeResourceDictionaryPostAction.Execute());
            Assert.Equal($"The style with key 'PageTitleStyle' is already defined with different value or elements in this file. Please review the styles to include the changes manually where required in your project.", ex.Message);
        }
    }
}
