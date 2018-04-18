using System;
using Caliburn.Micro;
using Param_ItemNamespace.Helpers;

namespace Param_ItemNamespace.ViewModels
{
    public class TabbedPivotPageViewModel : Conductor<Screen>.Collection.OneActive
    {
        public TabbedPivotPageViewModel()
        {
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            // WTS Add view models to the Items collection to display them in the Tabs
            Items.Add(new ExampleTabViewModel { DisplayName = "TabbedPivotPageExampleTabPage1_DisplayName".GetLocalized() });
            Items.Add(new ExampleTabViewModel { DisplayName = "TabbedPivotPageExampleTabPage1_DisplayName".GetLocalized() });
        }
    }
}
