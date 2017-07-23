// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Microsoft.Templates.UI.ViewModels.NewItem;

namespace Microsoft.Templates.UI.Views.NewItem
{
    /// <summary>
    /// Interaction logic for ProjectConfigurationWindow.xaml
    /// </summary>
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
