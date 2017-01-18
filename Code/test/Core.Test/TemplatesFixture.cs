using Microsoft.Templates.Core;

namespace Core.Test
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