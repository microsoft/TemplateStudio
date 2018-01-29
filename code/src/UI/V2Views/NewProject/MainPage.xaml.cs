// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Templates.UI.V2ViewModels.Common;
using Microsoft.Templates.UI.V2ViewModels.NewProject;

namespace Microsoft.Templates.UI.V2Views.NewProject
{
    public partial class MainPage : Page
    {
        private bool _handleSelection = true;
        private bool _listenComboUpdates = true;

        public MainPage()
        {
            DataContext = MainViewModel.Instance;
            InitializeComponent();
            V2Services.NavigationService.InitializeSecondaryFrame(stepFrame, new ProjectTypePage());
            V2Services.EventService.Instance.OnProjectTypeChange += OnProjectTypeChange;
            V2Services.EventService.Instance.OnFrameworkChange += OnFrameworkChange;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var listView = sender as ListView;
            if (listView != null)
            {
                V2Services.OrderingService.Initialize(listView);
            }
        }

        private void ComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null && !comboBox.IsDropDownOpen)
            {
                if (e.Key == Key.Left
                    || e.Key == Key.Up
                    || e.Key == Key.Right
                    || e.Key == Key.Down)
                {
                    e.Handled = true;
                }
            }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_listenComboUpdates)
            {
                if (_handleSelection)
                {
                    if (!MainViewModel.Instance.IsSelectionEnabled())
                    {
                        var combo = (ComboBox)sender;
                        _handleSelection = false;
                        if (e.RemovedItems != null && e.RemovedItems.Count > 0)
                        {
                            combo.SelectedItem = e.RemovedItems[0];
                            return;
                        }
                    }
                    else
                    {
                        if (e.AddedItems != null && e.AddedItems.Count > 0)
                        {
                            MainViewModel.Instance.ProcessItem(e.AddedItems[0]);
                            return;
                        }
                    }
                }

                _handleSelection = true;
            }
        }

        private void OnProjectTypeChange(object sender, MetadataInfoViewModel e)
        {
            _listenComboUpdates = false;
            projectTypeCombo.SelectedItem = e;
            _listenComboUpdates = true;
        }

        private void OnFrameworkChange(object sender, MetadataInfoViewModel e)
        {
            _listenComboUpdates = false;
            frameworkCombo.SelectedItem = e;
            _listenComboUpdates = true;
        }
    }
}
