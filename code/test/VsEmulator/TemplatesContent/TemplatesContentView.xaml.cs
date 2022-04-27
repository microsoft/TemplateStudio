// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;

namespace Microsoft.Templates.VsEmulator.TemplatesContent
{
    /// <summary>
    /// Interaction logic for TemplatesContentView.xaml
    /// </summary>
    public partial class TemplatesContentView : Window
    {
        public TemplatesContentViewModel ViewModel { get; set; }

        public TemplatesContentView(string wizardVersion)
        {
            ViewModel = new TemplatesContentViewModel(this, wizardVersion);

            InitializeComponent();

            DataContext = ViewModel;

            Loaded += (o, e) =>
            {
                ViewModel.Initialize();
            };
        }
    }
}
