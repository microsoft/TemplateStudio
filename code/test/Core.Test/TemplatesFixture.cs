using Microsoft.Templates.Core.Test.Locations;

namespace Microsoft.Templates.Core.Test
{
    public class TemplatesFixture
    {
        public TemplatesRepository Repository { get; }

        public TemplatesFixture()
        {
            var source = new UnitTestsTemplatesSource();
            CodeGen.Initialize(source.Id);
            Repository = new TemplatesRepository(source);
            
            Repository.SynchronizeAsync(true).Wait();
        }
    }
}