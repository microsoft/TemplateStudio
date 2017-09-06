using System;

using MixedNavigationSample.MVVMBasic.ViewModels;

using Windows.UI.Xaml.Controls;

namespace MixedNavigationSample.MVVMBasic.Views
{
    public sealed partial class StartPage : Page
    {
        public StartViewModel ViewModel { get; } = new StartViewModel();

        public StartPage()
        {
            InitializeComponent();
        }
    }
}
