using Param_ItemNamespace.Views;
using Param_ItemNamespace.Helpers;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace Param_ItemNamespace.Services
{
    public class FirstRunDisplayService : IFirstRunDisplayService
    {
        public async Task ShowIfAppropriateAsync()
        {
            bool hasShownFirstRun = false;
            hasShownFirstRun = await Windows.Storage.ApplicationData.Current.LocalSettings.ReadAsync<bool>(nameof(hasShownFirstRun));

            if (!hasShownFirstRun)
            {
                await Windows.Storage.ApplicationData.Current.LocalSettings.SaveAsync(nameof(hasShownFirstRun), true);
                var dialog = new FirstRunDialog();
                await dialog.ShowAsync();
            }
        }
    }
}
