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
        private static Dictionary<string, SampleCompany> items =
            new Dictionary<string, SampleCompany>();

        public ItemRepository()
        {
            Task.Run(async () =>
            {
                foreach (var company in await SampleDataService.GetWebApiSampleDataAsync())
                {
                    items.TryAdd(company.CompanyID, company);
                }
            });
        }

        public IEnumerable<SampleCompany> GetAll()
        {
            return items.Values;
        }

        public void Add(SampleCompany item)
        {
            items[item.CompanyID] = item;
        }

        public SampleCompany Get(string id)
        {
            items.TryGetValue(id, out SampleCompany item);

            return item;
        }

        public SampleCompany Remove(string id)
        {
            items.Remove(id, out SampleCompany item);

            return item;
        }

        public void Update(SampleCompany item)
        {
            items[item.CompanyID] = item;
        }
    }
}
