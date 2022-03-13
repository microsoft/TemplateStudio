using System.Collections.Generic;
using System.Linq;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Test;

namespace TemplateStudioForUWP.Tests
{
    public class UwpBaseGenAndBuildTests : BaseGenAndBuildTests
    {
        public UwpBaseGenAndBuildTests(BaseGenAndBuildFixture fixture, IContextProvider contextProvider = null, string framework = "")
            : base(fixture, contextProvider, framework)
        {
        }

        // This returns a list of project types and frameworks supported by BOTH C# and VB
        public static IEnumerable<object[]> GetMultiLanguageProjectsAndFrameworks()
        {
            // This list is hardcoded and not dynamically generated as it will rarely change and generation can be slow
            yield return new object[] { ProjectTypes.SplitView, Frameworks.CodeBehind };
            yield return new object[] { ProjectTypes.SplitView, Frameworks.MVVMToolkit };
            yield return new object[] { ProjectTypes.Blank, Frameworks.CodeBehind };
            yield return new object[] { ProjectTypes.Blank, Frameworks.MVVMToolkit };
            yield return new object[] { ProjectTypes.TabbedNav, Frameworks.CodeBehind };
            yield return new object[] { ProjectTypes.TabbedNav, Frameworks.MVVMToolkit };
            yield return new object[] { ProjectTypes.MenuBar, Frameworks.CodeBehind };
            yield return new object[] { ProjectTypes.MenuBar, Frameworks.MVVMToolkit };
        }

        // Set a single programming language to stop the fixture using all languages available to it
        public static new IEnumerable<object[]> GetProjectTemplatesForBuild(string framework, string programmingLanguage, string platform)
        {
            List<object[]> result = new List<object[]>();

            if (string.IsNullOrWhiteSpace(framework))
            {
                foreach (var fwork in Frameworks.All)
                {
                    result.AddRange(BuildTemplatesTestFixture.GetProjectTemplates(new UwpTestsTemplatesSource(), fwork, programmingLanguage, platform));
                }
            }
            else {
                result = BuildTemplatesTestFixture.GetProjectTemplates(new UwpTestsTemplatesSource(), framework, programmingLanguage, platform).ToList();
            }

            return result;
        }

        public static new IEnumerable<object[]> GetPageAndFeatureTemplatesForBuild(string framework, string language, string platform, string excludedItem = "")
        {
            IEnumerable<object[]> result = new List<object[]>();

            switch (framework)
            {
                case Frameworks.CodeBehind:
                    result = UwpBuildTemplatesTestFixture.GetPageAndFeatureTemplatesForBuild(framework, language, platform, excludedItem);
                    break;

                case Frameworks.MVVMToolkit:
                    result = UwpBuildTemplatesTestFixture.GetPageAndFeatureTemplatesForBuild(framework, language, platform, excludedItem);
                    break;

                case Frameworks.Prism:
                    result = UwpBuildTemplatesTestFixture.GetPageAndFeatureTemplatesForBuild(framework, language, platform, excludedItem);
                    break;
            }

            return result;
        }
    }
}
