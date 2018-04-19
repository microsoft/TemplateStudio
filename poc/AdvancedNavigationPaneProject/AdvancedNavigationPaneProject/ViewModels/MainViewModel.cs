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

        public ICommand ExpandCommand => _expandCommand ?? (_expandCommand = new RelayCommand(EnExpand));

        public MainViewModel()
        {
        }

        private void EnExpand()
        {
            NavigationService.Navigate<MainPage>();
        }
    }
}
