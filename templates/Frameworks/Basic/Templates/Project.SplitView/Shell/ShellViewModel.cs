using System;
using System.Collections.Generic;
using System.Windows.Input;

using ItemName.Mvvm;

//PostActionAnchor: ADD PAGE NS

namespace ItemName.Shell
{
    public class ShellViewModel : Observable
    {
        #region IsPaneOpen
        private bool _isPaneOpen;
        public bool IsPaneOpen
        {
            get => _isPaneOpen;
            set => Set(ref _isPaneOpen, value);
        }
        #endregion

        public ShellViewModel() 
        {
        }

        public IEnumerable<ShellNavigationItem> NavigationItems
        {
            get
            {
                //TODO: UWPTemplates -> Show pages in the SplitView menu by adding a navigation item for each page with its name and icon.
                //More on Segoe UI Symbol icons: https://docs.microsoft.com/windows/uwp/style/segoe-ui-symbol-font

                //PostActionAnchor: ADD PAGE TO NAVIGATION
                                
                //Edit String/en-US/Resources.resw: Add a menu item title for each page
            }
        }

        #region OpenPaneCommand
        private ICommand _openPaneCommand;
        public ICommand OpenPaneCommand
        {
            get
            {
                return _openPaneCommand ?? (_openPaneCommand = new RelayCommand(() => IsPaneOpen = !_isPaneOpen));
            }
        }
        #endregion
    }
}
