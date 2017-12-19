using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using WTSGeneratedNavigation.ViewModels;

namespace WTSGeneratedNavigation.Views
{
    // TODO WTS: This page exists purely as an example of how to launch a specific page in response to a protocol launch and pass it a value. It is expected that you will delete this page once you have changed the handling of a protocol launch to meet your needs and redirected to another of your pages.
    public sealed partial class UriSchemePage : Page
    {
        private UriSchemeViewModel ViewModel => DataContext as UriSchemeViewModel;

        public UriSchemePage()
        {
            InitializeComponent();
        }
    }
}
