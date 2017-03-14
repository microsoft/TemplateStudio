using Microsoft.Templates.Core.Test.Locations;

namespace Microsoft.Templates.Core.Test
{
    public class TemplatesFixture
    {
        public TemplatesRepository Repository { get; }

        public TemplatesFixture()
        {
            //TODO: Review
            var location = new UnitTestsTemplatesLocation();
            Repository = new TemplatesRepository(location);
            CodeGen.Initialize(location.Id);
            Repository.SynchronizeAsync(true).Wait();
        }
    }
}