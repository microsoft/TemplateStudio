using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Param_RootNamespace.Models;

namespace Param_RootNamespace.Views.Navigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailPage : Xamarin.Forms.MasterDetailPage
    {
        public MasterDetailPage()
        {
            InitializeComponent();

            MasterPage.PrimaryListView.ItemSelected += ListView_ItemSelected;

            if (Device.RuntimePlatform == Device.UWP)
            {
                MasterBehavior = MasterBehavior.Popover;
            }
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var menuItem = e.SelectedItem as MasterDetailPageMenuItem;

                var page = (Page)Activator.CreateInstance(menuItem.TargetType);
                var navPage = new NavigationPage(page);
                Detail = navPage;

                IsPresented = false;
            }

            (sender as ListView).SelectedItem = null;
        }
    }
}
