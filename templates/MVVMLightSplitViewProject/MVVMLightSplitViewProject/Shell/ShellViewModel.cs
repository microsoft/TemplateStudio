using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MVVMLightSplitViewProject.Home;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace MVVMLightSplitViewProject.Shell
{
    public class ShellViewModel : ViewModelBase
    {
        #region IsPaneOpen
        private bool _isPaneOpen;
        public bool IsPaneOpen
        {
            get => _isPaneOpen;
            set => Set(ref _isPaneOpen, value);
        }
        #endregion

        public ShellViewModel() { }

        public IEnumerable<ShellNavigationItem> NavigationItems
        {
            get
            {
                //TODO: Add new shell navigation items
                yield return new ShellNavigationItem("PaneHome", Char.ConvertFromUtf32(0xE80F), typeof(HomeViewModel).FullName);
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
