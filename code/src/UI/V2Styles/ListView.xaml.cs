using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Templates.UI.V2ViewModels.Common;

namespace Microsoft.Templates.UI.V2Styles
{
    public partial class ListView
    {
        private async void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null)
            {
                await BaseMainViewModel.BaseInstance.ProcessItemAsync(item.Content);
            }
        }

        private async void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var listView = sender as System.Windows.Controls.ListView;
            if (listView != null && e.Key == Key.Enter)
            {
                await BaseMainViewModel.BaseInstance.ProcessItemAsync(listView.SelectedItem);
            }
        }
    }
}
