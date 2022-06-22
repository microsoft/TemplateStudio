// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.Core.PostActions.Catalog.Merge.CodeStyleProviders;
using Microsoft.Templates.Core.Test.TestFakes;
using Microsoft.Templates.SharedResources;
using Xunit;

namespace Microsoft.Templates.Core.Test.PostActions.Catalog
{
    [Trait("Group", "Minimum")]
    public class MergeResourceDictionaryPostactionTest
    {
        [Fact]
        public void MergeResourceDictionary_Execute()
        {
            var source = Path.GetFullPath(@".\TestData\Merge\Style.xaml");
            var postaction = Path.GetFullPath(@".\TestData\Merge\Style_postaction.xaml");
            var expected = File.ReadAllText(@".\TestData\Merge\Style_expected.xaml").Replace("\r\n", string.Empty).Replace("\n", string.Empty);

            var config = new MergeConfiguration(postaction, new XmlStyleProvider(), true);

            GenContext.Current = new TestContextProvider()
            {
                GenerationOutputPath = Environment.CurrentDirectory,
                DestinationPath = Environment.CurrentDirectory,
            };

            var mergeResourceDictionaryPostAction = new MergeResourceDictionaryPostAction("Test", config);
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

            GenContext.Current = new TestContextProvider()
            {
                ProjectName = "TestResourceDictionaryPostAction",
                DestinationPath = Directory.GetCurrentDirectory(),
            };

            var config = new MergeConfiguration(postaction, new XmlStyleProvider(), true);

            var mergeResourceDictionaryPostAction = new MergeResourceDictionaryPostAction("TestTemplate", config);

            Exception ex = Assert.Throws<Exception>(() => mergeResourceDictionaryPostAction.Execute());
            Assert.NotNull(ex.InnerException);
            Assert.Equal(typeof(System.IO.InvalidDataException), ex.InnerException.GetType());
            Assert.Equal(string.Format(Resources.PostActionException, "Microsoft.Templates.Core.PostActions.Catalog.Merge.MergeResourceDictionaryPostAction", "TestTemplate"), ex.Message);
        }
    }
}
