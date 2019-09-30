using System;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Param_RootNamespace.Views;


namespace Param_RootNamespace.Services
{
    public static class FirstRunDisplayService
    {
        private static bool shown = false;

        internal static async Task ShowIfAppropriateAsync()
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal, async () =>
                {
                    if (SystemInformation.IsFirstRun && !shown)
                    {
                        shown = true;
                        var dialog = new FirstRunDialog();
                        await dialog.ShowAsync();
                    }
                });
        }
    }
}
