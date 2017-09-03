using System;
using Caliburn.Micro;

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
        }
    }
}
