﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;

namespace Microsoft.Templates.VsEmulator.NewProject
{
    /// <summary>
    /// Interaction logic for NewProjectView.xaml
    /// </summary>
    public partial class NewProjectView : Window
    {
        public NewProjectViewModel ViewModel { get; set; }

        public NewProjectView()
        {
            ViewModel = new NewProjectViewModel(this);
            InitializeComponent();

            DataContext = ViewModel;

            Loaded += (o, e) =>
            {
                ViewModel.Initialize();
            };
        }
    }
}
