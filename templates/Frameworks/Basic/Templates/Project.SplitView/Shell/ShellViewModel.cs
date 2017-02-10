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
                //PostActionAnchor: ADD PAGE TO NAVIGATION

                //TODO: UWPTemplates -> To show your project pages in the SplitView menu, add a navigation item for each page like above
                //i.e. yield return ShellNavigationItem.FromType<......>("Pane....", Char.ConvertFromUtf32(...));
                                
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
