// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using MarkdownViewer.ViewModels;

using Windows.UI.Xaml.Controls;

namespace MarkdownViewer.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();
        public MainPage()
        {
            InitializeComponent();
        }
    }
}
