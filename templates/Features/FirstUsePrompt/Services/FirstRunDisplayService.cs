using Param_ItemNamespace.Views;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Threading.Tasks;

namespace Param_ItemNamespace.Services
{
    public static class FirstRunDisplayService
    {
        internal static async Task ShowIfAppropriateAsync()
        {
            if (SystemInformation.IsFirstRun)
            {
                var dialog = new FirstRunDialog();
                await dialog.ShowAsync();
            }
        }
    }
}
