using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItemNamespace.MasterDetailPage
{
    public class SampleModelService
    {
        public async Task<IEnumerable<SampleModel>> GetDataAsync()
        {
            await Task.Delay(0);

            var data = new List<SampleModel>();

            data.Add(new SampleModel
            {
                Title = "Lorem ipsum dolor sit amet",
                Subtitle = "Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua",
                Link = "http://www.adatum.com/",
                Category = "Consectetur Adipiscing",
                Description = "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. At vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias excepturi sint occaecati cupiditate non provident, similique sunt in culpa qui officia deserunt mollitia animi, id est laborum et dolorum fuga. Et harum quidem rerum facilis est et expedita distinctio. Nam libero tempore, cum soluta nobis est eligendi optio cumque nihil impedit quo minus id quod maxime placeat facere possimus",
            });

            data.Add(new SampleModel
            {
                Title = "Sed ut perspiciatis unde",
                Subtitle = "Sit voluptatem accusantium doloremque laudantium, totam rem aperiam",
                Link = "http://www.adventure-works.com/",
                Category = "Iste Natus",
                Description = "Eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur?. omnis voluptas assumenda est, omnis dolor repellendus",

            });
            return data;
        }
    }
}
