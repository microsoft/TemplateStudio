using System;
using System.Threading.Tasks;

using Windows.ApplicationModel;

using WTSGeneratedNavigation.Helpers;
using WTSGeneratedNavigation.Views;

namespace WTSGeneratedNavigation.Services
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
