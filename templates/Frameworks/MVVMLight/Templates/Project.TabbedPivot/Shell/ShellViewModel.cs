using System;
using System.Collections.Generic;
using System.Windows.Input;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace uct.ItemName.Shell
{
    public class ShellViewModel : ViewModelBase
    {
        public ShellViewModel() 
        {
        }

        public IEnumerable<ShellTabbedItem> Items
        {
            get
            {
                //TODO: UWPTemplates -> Show pages in Pivot by adding a navigation item for each page with its name.                               
                //Edit String/en-US/Resources.resw: Add a menu item title for each page
            }
        }
    }
}
