namespace Param_RootNamespace.Core.Services
{
    public class SampleDataService : ISampleDataService
    {
//^^
//{[{

        public async Task<IEnumerable<SampleOrder>> GetGridDataAsync()
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