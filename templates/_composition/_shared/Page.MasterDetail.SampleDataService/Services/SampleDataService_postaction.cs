using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Param_ItemNamespace.Models;
using Windows.UI.Xaml.Controls;

namespace Param_ItemNamespace.Services
{
    public static class SampleDataService
    {
//^^
//{[{
        // TODO UWPTemplates: Remove this once your MasterDetail pages are displaying real data
        public static async Task<IEnumerable<SampleModel>> GetSampleModelDataAsync()
        {
            await Task.CompletedTask;
            var data = new List<SampleModel>();

            data.Add(new SampleModel
            {
                Title = "Lorem ipsum dolor sit 1",
                Description = "Lorem ipsum dolor sit amet",
                Symbol = Symbol.Globe
            });

            data.Add(new SampleModel
            {
                Title = "Lorem ipsum dolor sit 2",
                Description = "Lorem ipsum dolor sit amet",
                Symbol = Symbol.MusicInfo
            });
            return data;
        }
//}]}

    }
}
