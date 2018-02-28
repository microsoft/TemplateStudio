// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows.Controls;
using Microsoft.Templates.UI.ViewModels.NewItem;

namespace Microsoft.Templates.UI.Views.NewItem
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            DataContext = MainViewModel.Instance;
            InitializeComponent();
        }

        private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Services.NavigationService.InitializeSecondaryFrame(stepFrame, new TemplateSelectionPage());
        }

        private void OnUnloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Services.NavigationService.UnsuscribeEventHandlers();
        }
    }
}
