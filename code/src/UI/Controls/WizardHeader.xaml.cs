// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Microsoft.Templates.Core.Mvvm;

namespace Microsoft.Templates.UI.Controls
{
    public partial class WizardHeader : UserControl
    {
        public string HeaderText
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }

        public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register(nameof(HeaderText), typeof(string), typeof(WizardHeader), new PropertyMetadata(string.Empty));

        public bool HasOverlayBox
        {
            get { return (bool)GetValue(HasOverlayBoxProperty); }
            set { SetValue(HasOverlayBoxProperty, value); }
        }

        public static readonly DependencyProperty HasOverlayBoxProperty = DependencyProperty.Register(nameof(HasOverlayBox), typeof(bool), typeof(WizardHeader), new PropertyMetadata(false));

        public bool IsOverlayBoxVisible
        {
            get { return (bool)GetValue(IsOverlayBoxVisibleProperty); }
            set { SetValue(IsOverlayBoxVisibleProperty, value); }
        }

        public static readonly DependencyProperty IsOverlayBoxVisibleProperty = DependencyProperty.Register(nameof(IsOverlayBoxVisible), typeof(bool), typeof(WizardHeader), new PropertyMetadata(false));

        public bool NewVersionAvailable
        {
            get { return (bool)GetValue(NewVersionAvailableProperty); }
            set { SetValue(NewVersionAvailableProperty, value); }
        }

        public static readonly DependencyProperty NewVersionAvailableProperty = DependencyProperty.Register(nameof(NewVersionAvailable), typeof(bool), typeof(WizardHeader), new PropertyMetadata(false));

        public ICommand ShowOverlayMenuCommand { get; set; }

        public WizardHeader()
        {
            InitializeComponent();
            ShowOverlayMenuCommand = new RelayCommand(OnShowOverlayMenuCommand);
        }

        private void OnShowOverlayMenuCommand()
        {
            IsOverlayBoxVisible = !IsOverlayBoxVisible;
        }
    }
}
