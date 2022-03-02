// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
