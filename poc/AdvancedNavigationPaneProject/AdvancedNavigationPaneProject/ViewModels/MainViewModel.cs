using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AdvancedNavigationPaneProject.Helpers;
using AdvancedNavigationPaneProject.Services;
using AdvancedNavigationPaneProject.Views;

namespace AdvancedNavigationPaneProject.ViewModels
{
    public class MainViewModel : Observable
    {
        private ICommand _expandCommand;

        public ICommand ExpandCommand => _expandCommand ?? (_expandCommand = new RelayCommand(OnExpand, CanExpand));

        public MainViewModel()
        {
        }

        private void OnExpand()
        {
            NavigationService.Navigate<MainPage>();
        }

        private bool CanExpand()
        {
            var mainFrame = NavigationService.GetFrame(NavigationService.FrameKeyMain);
            return mainFrame.Content != null && mainFrame.Content?.GetType() != typeof(MainPage);
        }
    }
}
