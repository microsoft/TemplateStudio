// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;

using Microsoft.Templates.UI.ViewModels.NewItem;

namespace Microsoft.Templates.UI.Views.NewItem
{
    public partial class ProjectConfigurationWindow : Window
    {
        public ProjectConfigurationViewModel ViewModel { get; }

        public ProjectConfigurationWindow(Window mainWindow)
        {
            ViewModel = new ProjectConfigurationViewModel(this);
            DataContext = ViewModel;
            Owner = mainWindow;
            Loaded += ProjectConfigurationWindow_Loaded;

            InitializeComponent();
        }

        private void ProjectConfigurationWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Initialize();
        }
    }
}
