using Microsoft.Templates.Core.Test.Locations;

namespace Microsoft.Templates.Core.Test
{
    public class TemplatesFixture
    {
        public TemplatesRepository Repository { get; }

        public TemplatesFixture()
        {
            Repository = new TemplatesRepository(new UnitTestsTemplatesLocation());
            Repository.Sync();
        }
    }
}