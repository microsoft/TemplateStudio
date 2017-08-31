// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;

using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.Views.Common
{
    /// <summary>
    /// Interaction logic for ErrorDialog.xaml
    /// </summary>
    public partial class ErrorDialog : Window
    {
        public ErrorViewModel ViewModel { get; }

        public ErrorDialog(Exception ex)
        {
            ViewModel = new ErrorViewModel(this, ex);
            DataContext = ViewModel;

            InitializeComponent();
        }
    }
}
