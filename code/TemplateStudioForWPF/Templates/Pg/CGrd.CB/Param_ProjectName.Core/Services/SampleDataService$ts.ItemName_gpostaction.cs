﻿namespace Param_RootNamespace.Core.Services
{
    public class SampleDataService : ISampleDataService
    {
//^^
//{[{

        // Remove this once your ContentGrid pages are displaying real data.
        public async Task<IEnumerable<SampleOrder>> GetContentGridDataAsync()
        {
            await Task.CompletedTask;
            return AllOrders();
        }
//}]}
    }
}