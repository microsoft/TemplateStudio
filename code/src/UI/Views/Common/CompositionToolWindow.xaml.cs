// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.ViewModels.NewProject;

namespace Microsoft.Templates.UI.Views.Common
{
    public partial class CompositionToolWindow : Window
    {
        private readonly UserSelection _userSelection;

        public CompositionToolWindow(UserSelection userSelection)
        {
            _userSelection = userSelection;
            DataContext = MainViewModel.Instance.CompositionTool;
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            MainViewModel.Instance.CompositionTool.Initialize(_userSelection);
        }

        private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            MainViewModel.Instance.CompositionTool.SelectItem(e.NewValue);
        }

        private void OnKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
