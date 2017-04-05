using ItemNamespace.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace ItemNamespace.Services
{
    public class SampleModelService
    {
        public async Task<IEnumerable<SampleModel>> GetDataAsync()
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
    }
}
