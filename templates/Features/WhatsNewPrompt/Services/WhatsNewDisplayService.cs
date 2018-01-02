using Param_ItemNamespace.Views;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Threading.Tasks;

namespace Param_ItemNamespace.Services
{
    // For instructions on testing this service see https://github.com/Microsoft/WindowsTemplateStudio/tree/master/docs/features/whats-new-prompt.md
    public static class WhatsNewDisplayService
    {
        internal static async Task ShowIfAppropriateAsync()
        {
            if (SystemInformation.IsAppUpdated)
            {
                var dialog = new WhatsNewDialog();
                await dialog.ShowAsync();
            }
        }
    }
}
