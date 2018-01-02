using Param_ItemNamespace.Views;
using Param_ItemNamespace.Helpers;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace Param_ItemNamespace.Services
{
    public class FirstRunDisplayService : IFirstRunDisplayService
    {
        public async Task ShowIfAppropriateAsync()
        {
            if (SystemInformation.IsFirstRun)
            {
                var dialog = new FirstRunDialog();
                await dialog.ShowAsync();
            }
        }
    }
}
