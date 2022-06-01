namespace Param_RootNamespace.Core.Services;

public class SampleDataService : ISampleDataService
{
//^^
//{[{

    public async Task SaveOrderAsync(SampleOrder order)
    {
        if (_allOrders == null)
        {
            _allOrders = new List<SampleOrder>(AllOrders());
        }

        _allOrders.Add(order);
        await Task.CompletedTask;
    }
//}]}
}
