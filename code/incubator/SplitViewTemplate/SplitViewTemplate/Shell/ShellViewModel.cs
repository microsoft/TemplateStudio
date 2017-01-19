using System.Collections.Generic;

using SplitViewTemplate.Model;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace SplitViewTemplate.ViewModel
{
    public class ShellViewModel : ViewModel
    {
        #region IsPaneOpen
        private bool _isPaneOpen;
        public bool IsPaneOpen
        {
            get { return _isPaneOpen; }
            set { Set(ref _isPaneOpen, value); }
        }
        #endregion

        public ShellViewModel(Services.INavigationService navigationService) : base(navigationService)
        {

        }

        public IEnumerable<ShellNavigationItem> NavigationItems
        {
            get
            {
                yield return ShellNavigationItem.FromIcon("ms-appx:///Assets/Icons/Home.png", ViewModelLocator.HomeViewKey, "PaneHome");
                yield return ShellNavigationItem.FromIcon("ms-appx:///Assets/Icons/PowerPoint.png", ViewModelLocator.PowerPointViewKey, "PaneMicrosoftPowerPoint");
                yield return ShellNavigationItem.FromIcon("ms-appx:///Assets/Icons/Excel.png", ViewModelLocator.ExcelViewKey, "PaneMicrosoftExcel");
                yield return ShellNavigationItem.FromIcon("ms-appx:///Assets/Icons/Word.png", ViewModelLocator.WordViewKey, "PaneMicrosoftWord");
                yield return ShellNavigationItem.FromGlyph("", ViewModelLocator.AboutViewKey, "PaneAbout");
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
