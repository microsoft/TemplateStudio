using Microsoft.Templates.Core.Locations;

namespace Microsoft.UI.Test.ProjectConfigurationTests
{
    public sealed class UiTestsTemplatesSource : LocalTemplatesSource
    {
        public UiTestsTemplatesSource(string installedPackagePath)
            : base(installedPackagePath)
        {
        }

        public override string Id => "UnitTest" + GetAgentName();

        protected override string Origin => $@"..\..\..\UI.Test\TestData\{TemplatesFolderName}";
    }
}
