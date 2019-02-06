using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.UI.Controls;

namespace Param_RootNamespace.Views
{
    public sealed partial class MasterDetailViewPage : Page
    {
        public MasterDetailViewPage()
        {
            InitializeComponent();
        }

        private void MasterDetailsViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (MasterDetailsViewControl.ViewState == MasterDetailsViewState.Both)
            {
                ViewModel.ActiveItem = ViewModel.Items.FirstOrDefault();
            }
        }
    }
}
