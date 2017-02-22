using Microsoft.Templates.Core.Injection.References;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.Templates.Core.Test.Injection
{
    public class ReferencesInjectorTest
    {
        private const string Json = @"{
                                      ""dependencies"": {
                                        ""Microsoft.NETCore.UniversalWindowsPlatform"": ""5.2.2"",
                                        ""Microsoft.Xaml.Behaviors.Uwp.Managed"": ""1.0.3""
                                      },
                                      ""frameworks"": {
                                        ""uap10.0"": {}
                                      },
                                      ""runtimes"": {
                                        ""win10-arm"": {},
                                        ""win10-arm-aot"": {},
                                        ""win10-x86"": {},
                                        ""win10-x86-aot"": {},
                                        ""win10-x64"": {},
                                        ""win10-x64-aot"": {}
                                      }
                                    }";

        private const string ExpectedJson = @"{
                                      ""dependencies"": {
                                        ""Microsoft.NETCore.UniversalWindowsPlatform"": ""5.2.2"",
                                        ""Microsoft.Xaml.Behaviors.Uwp.Managed"": ""1.0.3"",
                                        ""MvvmLight"": ""5.3.0""
                                      },
                                      ""frameworks"": {
                                        ""uap10.0"": {}
                                      },
                                      ""runtimes"": {
                                        ""win10-arm"": {},
                                        ""win10-arm-aot"": {},
                                        ""win10-x86"": {},
                                        ""win10-x86-aot"": {},
                                        ""win10-x64"": {},
                                        ""win10-x64-aot"": {}
                                      }
                                    }";


        private const string ExpectedJsonEmpty = @"{
                                      ""dependencies"": {
                                        ""MvvmLight"": ""5.3.0""
                                      }
                                    }";

        [Fact]
        public void Inject()
        {
            var config = new ReferencesInjectorConfig
            {
                dependencies = new Dictionary<string, string>
                {
                    { "MvvmLight", "5.3.0" }
                }
            };

            var target = new ReferencesInjector(config);

            var result = target.Inject(Json);
            var expected = JsonConvert.DeserializeObject<ProjectJson>(ExpectedJson);

            Assert.Equal(JsonConvert.SerializeObject(expected, Formatting.Indented), result);
        }

        [Fact]
        public void Inject_EmptyDependencies()
        {
            var config = new ReferencesInjectorConfig
            {
                dependencies = new Dictionary<string, string>
                {
                    { "MvvmLight", "5.3.0" }
                }
            };

            var target = new ReferencesInjector(config);

            var result = target.Inject("{}");
            var expected = JsonConvert.DeserializeObject<ProjectJson>(ExpectedJsonEmpty);

            Assert.Equal(JsonConvert.SerializeObject(expected, Formatting.Indented), result);
        }
    }
}
