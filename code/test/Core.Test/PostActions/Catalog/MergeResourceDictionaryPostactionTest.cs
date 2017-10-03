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
        public string ProjectName => throw new NotImplementedException();

        public string OutputPath => Directory.GetCurrentDirectory();

        public string ProjectPath => throw new NotImplementedException();

        public List<string> ProjectItems => throw new NotImplementedException();

        public List<string> FilesToOpen => throw new NotImplementedException();

        public List<FailedMergePostAction> FailedMergePostActions => throw new NotImplementedException();

        public Dictionary<string, List<MergeInfo>> MergeFilesFromProject => throw new NotImplementedException();

        public Dictionary<ProjectMetricsEnum, double> ProjectMetrics => throw new NotImplementedException();

        [Fact]
        public void MergeResourceDictionaryPostaction()
        {
            var source = Path.GetFullPath(@".\TestData\Merge\Style.xaml");
            var postaction = Path.GetFullPath(@".\TestData\Merge\Style_postaction.xaml");
            var expected = File.ReadAllText(@".\TestData\Merge\Style_expected.xaml").Replace("\r\n", "");

            var config = new MergeConfiguration(postaction, true);

            var mergeResourceDictionaryPostAction = new MergeResourceDictionaryPostAction(config);
            mergeResourceDictionaryPostAction.Execute();

            var result = File.ReadAllText(source).Replace("\r\n", "");

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

            Assert.Equal($"Key PageTitleStyle already defined with different value or elements in file '{source.Replace(GenContext.Current.OutputPath + Path.DirectorySeparatorChar, "")}'", ex.Message);
        }
    }
}
