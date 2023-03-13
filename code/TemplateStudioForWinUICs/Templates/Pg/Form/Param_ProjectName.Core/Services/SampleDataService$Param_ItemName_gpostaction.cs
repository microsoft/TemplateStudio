namespace Param_RootNamespace.Core.Services;

public class SampleDataService : ISampleDataService
{
//^^
//{[{

    public async Task SaveOrderAsync(SampleOrder order)
    {
        _allOrders ??= new List<SampleOrder>(AllOrders());

        _allOrders.Add(order);
        await Task.CompletedTask;
    }
//}]}
}
