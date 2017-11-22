// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Input;

namespace Microsoft.Templates.VsEmulator.LoadProject
{
    /// <summary>
    /// Interaction logic for LoadProjectView.xaml
    /// </summary>
    public partial class LoadProjectView : Window
    {
        public LoadProjectViewModel ViewModel { get; set; }

        public LoadProjectView(string solutionPath)
        {
            ViewModel = new LoadProjectViewModel(this);
            InitializeComponent();

            DataContext = ViewModel;

            Loaded += (o, e) =>
            {
                ViewModel.Initialize(solutionPath);
                okButton.Focus();
            };
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
