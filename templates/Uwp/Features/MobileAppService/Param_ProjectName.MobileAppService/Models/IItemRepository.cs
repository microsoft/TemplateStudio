using System;
using System.Collections.Generic;
using Param_RootNamespace.Core.Models;

namespace Param_RootNamespace.Models
{
    public interface IItemRepository
    {
        void Add(Item item);
        void Update(Item item);
        Item Remove(string key);
        Item Get(string id);
        IEnumerable<Item> GetAll();
    }
}
