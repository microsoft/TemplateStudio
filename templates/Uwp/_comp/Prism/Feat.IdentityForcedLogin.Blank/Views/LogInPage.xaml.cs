using System;
using Param_RootNamespace.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Param_RootNamespace.Views
{
    public sealed partial class LogInPage : Page
    {
        private LogInViewModel ViewModel => DataContext as LogInViewModel;

        public LogInPage()
        {
            InitializeComponent();
        }
    }
}
