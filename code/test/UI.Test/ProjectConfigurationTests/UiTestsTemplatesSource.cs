using Microsoft.Templates.Core.Locations;

namespace Microsoft.UI.Test.ProjectConfigurationTests
{
    public sealed class UITestsTemplatesSource : LocalTemplatesSource
    {
        public UITestsTemplatesSource(string installedPackagePath)
            : base(installedPackagePath)
        {
        }

        public override string Id => "UITest" + GetAgentName();

        protected override string Origin => $@"..\..\..\UI.Test\TestData\{TemplatesFolderName}";
    }
}
