using System;
using System.Collections.Generic;
using Param_RootNamespace.Core.Models;

namespace Param_RootNamespace.WebApi.Models
{
    // TODO WTS: Replace or update this interface when no longer using sample data.
    public interface IItemRepository
    {
        void Add(SampleOrder item);

        void Update(SampleOrder item);

        SampleOrder Remove(long id);

        SampleOrder Get(long id);

        IEnumerable<SampleOrder> GetAll();
    }
}
