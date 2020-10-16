namespace Param_RootNamespace.Core.Services
{
    public class SampleDataService : ISampleDataService
    {
//^^
//{[{

        // TODO WTS: Remove this once your ContentGrid pages are displaying real data.
        public async Task<IEnumerable<SampleOrder>> GetContentGridDataAsync()
        {
            if (_allOrders == null)
            {
                _allOrders = new List<SampleOrder>(AllOrders());
            }

            await Task.CompletedTask;
            return _allOrders;
        }
//}]}
    }
}