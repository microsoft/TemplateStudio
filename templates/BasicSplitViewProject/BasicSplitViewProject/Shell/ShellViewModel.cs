using BasicSplitViewProject.Core;
using BasicSplitViewProject.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BasicSplitViewProject.Shell
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
                yield return ShellNavigationItem.FromType<HomePage>("PaneHome", Char.ConvertFromUtf32(0xE80F));

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
