using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Microsoft.Templates.UI.Extensions
{
    static public class ListViewExtensions
    {
        static public ListViewItem GetCurrentListViewItem(this ListView listView)
        {
            return listView.GetListViewItem(listView.SelectedIndex);
        }
        static public ListViewItem GetListViewItem(this ListView listView, int index)
        {
            if (listView.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
            {
                return null;
            }
            return listView.ItemContainerGenerator.ContainerFromIndex(index) as ListViewItem;
        }
    }
}
