using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Param_RootNamespace.Core.Models;
using Param_RootNamespace.Core.Services;

namespace Param_RootNamespace.WebApi.Models
{
    // TODO WTS: Replace or update this class when no longer using sample data.
    public class ItemRepository : IItemRepository
    {
        private static Dictionary<long, SampleOrder> items =
            new Dictionary<long, SampleOrder>();

        public ItemRepository()
        {
            Task.Run(async () =>
            {
                var dataService = new SampleDataService();

                foreach (var order in await dataService.GetWebApiSampleDataAsync())
                {
                    items.TryAdd(order.OrderId, order);
                }
            });
        }

        public IEnumerable<SampleOrder> GetAll()
        {
            return items.Values;
        }

        public void Add(SampleOrder item)
        {
            items[item.OrderId] = item;
        }

        public SampleOrder Get(long id)
        {
            items.TryGetValue(id, out SampleOrder item);

            return item;
        }

        public SampleOrder Remove(long id)
        {
            items.Remove(id, out SampleOrder item);

            return item;
        }

        public void Update(SampleOrder item)
        {
            items[item.OrderId] = item;
        }
    }
}
