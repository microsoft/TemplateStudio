using Param_RootNamespace.Views;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Threading.Tasks;

namespace Param_RootNamespace.Services
{
    public class FirstRunDisplayService : IFirstRunDisplayService
    {
        private static bool shown = false;

        public async Task ShowIfAppropriateAsync()
        {
            if (SystemInformation.IsFirstRun && !shown)
            {
                shown = true;
                var dialog = new FirstRunDialog();
                await dialog.ShowAsync();
            }
        }
    }
}
