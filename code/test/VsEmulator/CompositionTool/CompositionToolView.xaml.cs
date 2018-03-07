// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Controls;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;

namespace Microsoft.Templates.VsEmulator
{
    /// <summary>
    /// Interaction logic for CompositionTool.xaml
    /// </summary>
    public partial class CompositionToolView : Window
    {
        public CompositionToolViewModel ViewModel { get; } = new CompositionToolViewModel();

        public CompositionToolView()
        {
            DataContext = ViewModel;
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadData();
        }

        private void MetadataInfoListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listView = sender as ListView;
            if (listView != null)
            {
                var item = listView.SelectedItem as Selectable<MetadataInfo>;
                if (item != null)
                {
                    if (item.Item.MetadataType == MetadataType.ProjectType)
                    {
                        ViewModel.ProjectType = item.Item;
                    }
                    else
                    {
                        ViewModel.Framework = item.Item;
                    }
                }
            }
        }

        private void ITemplateInfoListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listView = sender as ListView;
            if (listView != null)
            {
                var item = listView.SelectedItem as Selectable<ITemplateInfo>;
                if (item != null)
                {
                    ViewModel.Template = item.Item;
                }
            }
        }
    }
}
