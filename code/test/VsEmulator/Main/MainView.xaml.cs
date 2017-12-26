// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Input;

namespace Microsoft.Templates.VsEmulator.Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainViewModel ViewModel { get; set; }

        public MainView()
        {
            ViewModel = new MainViewModel(this);
            InitializeComponent();

            DataContext = ViewModel;

            Loaded += async (sender, e) =>
            {
                await ViewModel.InitializeAsync();
            };
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
