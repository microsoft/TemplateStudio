using Microsoft.Templates.Wizard;
using Microsoft.Templates.Wizard.PostActions;
using Microsoft.Templates.Wizard.PostActions.Catalog;
using System;
using System.Collections.Generic;
using System.Configuration;
using Xunit;

namespace Wizard.Test
{
    public class InsertPartialGenerationPostActionTest
    {
        [Fact]
        public void InsertPartialGenerationPostAction_Ok()
        {
            var parameters = new Dictionary<string, string>
            {
                { "destination.filename", "TestWithAnchor.txt" },
                { "destination.anchor", "//PostActionAnchor" }
            };

            var genInfo = new GenInfo() { Name = "testpage" };

            var pa = new InsertPartialGenerationPostAction("TestPostAction", "testAction", parameters);

            var result = pa.Execute("test", genInfo, null, null);

            Assert.True(result.ResultCode == ResultCode.Success);
          
        }


        [Fact]
        public void InsertPartialGenerationPostAction_DestFileNameEmpty()
        {
            var parameters = new Dictionary<string, string>
            {
                { "destination.filename", "" },
                { "destination.anchor", "//PostActionAnchor" }
            };

            var genInfo = new GenInfo() { Name = "testpage" };

            var pa = new InsertPartialGenerationPostAction("TestPostAction", "testAction", parameters);

            Assert.Throws(typeof(ConfigurationErrorsException), () => pa.Execute("test", genInfo, null, null));
        }

        [Fact]
        public void InsertPartialGenerationPostAction_AnchorEmpty()
        {
            var parameters = new Dictionary<string, string>
            {
                { "destination.filename", "TestWithAnchor.txt" },
                { "destination.anchor", "" }
            };

            var genInfo = new GenInfo() { Name = "testpage" };

            var pa = new InsertPartialGenerationPostAction("TestPostAction", "testAction", parameters);

            Assert.Throws(typeof(ConfigurationErrorsException), () => pa.Execute("test", genInfo, null, null));
        }

        [Fact]
        public void InsertPartialGenerationPostAction_AnchorNotFound()
        {
            var parameters = new Dictionary<string, string>
            {
                { "destination.filename", "TestWithoutAnchor.txt" },
                { "destination.anchor", "//PostActionAnchor" }
            };

            var genInfo = new GenInfo() { Name = "testpage" };

            var pa = new InsertPartialGenerationPostAction("TestPostAction", "testAction", parameters);

            var result = pa.Execute("test", genInfo, null, null);

            Assert.True(result.ResultCode == ResultCode.AnchorNotFound);

        }

    }
}
