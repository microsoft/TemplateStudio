// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions.Catalog.AddJsonDictionaryItem;
using Microsoft.Templates.Core.Test.TestFakes;
using Newtonsoft.Json;
using Xunit;

namespace Microsoft.Templates.Core.Test.PostActions.Catalog
{
    [Trait("Group", "Minimum")]
    public class AddJsonDictionaryItemPostActionTest
    {
        [Fact]
        public void AddJsonDictionaryItem_Execute()
        {
            var templateName = "Test";
            var destPath = Path.GetFullPath(@".\TestData\TestJson");
            var expectedJsonPath = "TestJson_expected.json";
            var jsonPath = "TestJson.json";
            var key = "TestKey";

            var dictItemsToAdd = new Dictionary<string, string>()
            {
                { "abc", "123" },
                { "def", "456" },
                { "ghi", "789" },
                { "jkl", "111" },
            };

            string dictItemsToAddSerialized = JsonConvert.SerializeObject(dictItemsToAdd);

            GenContext.Current = new TestContextProvider()
            {
                DestinationPath = destPath,
                GenerationOutputPath = destPath,
            };

            var postAction = new FakeTemplateDefinedPostAction(
                new Guid(AddJsonDictionaryItemPostAction.Id),
                new Dictionary<string, string>()
                {
                    { "dict",  dictItemsToAddSerialized },
                    { "jsonPath", jsonPath },
                    { "key", key },
                });

            var mergePostAction = new AddJsonDictionaryItemPostAction(templateName, postAction, new Dictionary<string, string>(), destPath);
            mergePostAction.Execute();

            var combinedPathOfResult = Path.Combine(destPath, jsonPath);
            var combinedPathOfExpected = Path.Combine(destPath, expectedJsonPath);

            Assert.Equal(File.ReadAllText(combinedPathOfExpected), File.ReadAllText(combinedPathOfResult));
        }
    }
}
