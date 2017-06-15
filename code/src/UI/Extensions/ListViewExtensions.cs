using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Microsoft.Templates.UI.Extensions
{
    public static class ListViewExtensions
    {
        public static ListViewItem GetCurrentListViewItem(this ListView listView)
        {
            return listView.GetListViewItem(listView.SelectedIndex);
        }
        public static ListViewItem GetListViewItem(this ListView listView, int index)
        {
            if (listView.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
            {
                return null;
            }
            return listView.ItemContainerGenerator.ContainerFromIndex(index) as ListViewItem;
        }
    }
}
