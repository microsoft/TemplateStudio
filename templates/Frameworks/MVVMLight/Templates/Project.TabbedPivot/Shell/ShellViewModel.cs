using System;
using System.Collections.Generic;
using System.Windows.Input;
using ItemName.Home;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

//PostActionAnchor: ADD PAGE NS

namespace ItemName.Shell
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
                //TODO UWPTemplates: Show pages in Pivot by adding a navigation item for each page with its name.

                //PostActionAnchor: ADD PAGE TO NAVIGATION
                                
                //Edit String/en-US/Resources.resw: Add a menu item title for each page
            }
        }
    }
}
