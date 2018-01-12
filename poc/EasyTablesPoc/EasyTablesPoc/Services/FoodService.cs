using EasyTablesPoc.Models;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace EasyTablesPoc.Services
{
    public class FoodService : EasyTableService<Food>
    {
        protected override async Task ResolveErrorAsync(MobileServiceTableOperationError result)
        {
            if (result.Result == null || result.Item == null)
                return;

            var serverItem = result.Result.ToObject<Food>();
            var localItem = result.Item.ToObject<Food>();

            if (serverItem.Id == localItem.Id &&
                serverItem.Name == localItem.Name &&
                serverItem.Category == localItem.Category)
            {
                // The elements are equals, ignore the conflict
                await result.CancelAndDiscardItemAsync();
            }
            else
            {
                // Client win
                localItem.Version = serverItem.Version;
                await result.UpdateOperationAsync(JObject.FromObject(localItem));
            }
        }
    }
}
