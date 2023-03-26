namespace Param_RootNamespace.Core.Services;

public class SampleDataService : ISampleDataService
{
//^^
//{[{

    public async Task<IEnumerable<SampleOrder>> GetContentGridDataAsync()
    {
        _allOrders ??= new List<SampleOrder>(AllOrders());

        await Task.CompletedTask;
        return _allOrders;
    }
//}]}
}
