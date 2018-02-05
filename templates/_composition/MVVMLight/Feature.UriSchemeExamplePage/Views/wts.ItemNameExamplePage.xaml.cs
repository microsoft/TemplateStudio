using System;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Param_ItemNamespace.ViewModels;

namespace Param_ItemNamespace.Views
{
    // TODO WTS: Remove this example page when/if it's not needed.
    // This page is an example of how to launch a specific page in response to a protocol launch and pass it a value.
    // It is expected that you will delete this page once you have changed the handling of a protocol launch to meet
    // your needs and redirected to another of your pages.
    public sealed partial class wts.ItemNameExamplePage : Page
    {
        private wts.ItemNameExampleViewModel ViewModel
        {
            get { return DataContext as wts.ItemNameExampleViewModel; }
        }

        public wts.ItemNameExamplePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Capture the passed in value and assign it to a property that's displayed on the view
            ViewModel.Secret = e.Parameter.ToString();
        }
    }
}
