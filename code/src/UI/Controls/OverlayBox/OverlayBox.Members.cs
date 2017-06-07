using Microsoft.Templates.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Microsoft.Templates.UI.Controls
{
    public sealed partial class OverlayBox
    {
        public string WizardVersion
        {
            get => (string)GetValue(WizardVersionProperty);
            set => SetValue(WizardVersionProperty, value);
        }
        public static readonly DependencyProperty WizardVersionProperty = DependencyProperty.Register("WizardVersion", typeof(string), typeof(OverlayBox), new PropertyMetadata(String.Empty));

        public string TemplatesVersion
        {
            get => (string)GetValue(TemplatesVersionProperty);
            set => SetValue(TemplatesVersionProperty, value);
        }
        public static readonly DependencyProperty TemplatesVersionProperty = DependencyProperty.Register("TemplatesVersion", typeof(string), typeof(OverlayBox), new PropertyMetadata(String.Empty));

        public bool NewUpdateAvailable
        {
            get => (bool)GetValue(NewUpdateAvailableProperty);
            set => SetValue(NewUpdateAvailableProperty, value);
        }
        public static readonly DependencyProperty NewUpdateAvailableProperty = DependencyProperty.Register("NewUpdateAvailable", typeof(bool), typeof(OverlayBox), new PropertyMetadata(true));

        public ICommand OpenUrlCommand
        {
            get => (ICommand)GetValue(OpenUrlCommandProperty);
        }
        public static readonly DependencyProperty OpenUrlCommandProperty = DependencyProperty.Register("OpenUrlCommand", typeof(ICommand), typeof(OverlayBox), new PropertyMetadata(new RelayCommand<string>(OpenUrl)));

        private static void OpenUrl(string url)
        {
            if (!string.IsNullOrWhiteSpace(url) && Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                Process.Start(url);
            }
        }
    }
}
