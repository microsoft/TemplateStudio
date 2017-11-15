// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Input;
using Microsoft.Templates.UI.V2Services;
using Microsoft.Templates.UI.V2ViewModels.NewProject;
using Microsoft.VisualStudio.PlatformUI;

namespace Microsoft.Templates.UI.V2Views.NewProject
{
    public partial class MainView : Window
    {
        // private BaseColorService _colorService;
        public MainViewModel ViewModel { get; } = new MainViewModel();

        public UserSelection Result { get; set; }

        public MainView(string language)
        {
            DataContext = ViewModel;
            Loaded += (sender, args) =>
            {
                ViewModel.LoadData();
            };
            InitializeComponent();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
