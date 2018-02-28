using System;
using Windows.UI.Xaml.Controls;
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
    }
}
