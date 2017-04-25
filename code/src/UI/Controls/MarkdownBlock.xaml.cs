using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Microsoft.Templates.UI.Controls
{
    public partial class MarkdownBlock : UserControl
    {
        public MarkdownBlock()
        {
            InitializeComponent();

            CommandBindings.Add(new CommandBinding(NavigationCommands.GoToPage, (sender, e) => SafeNavigate(e.Parameter)));
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(MarkdownBlock), new PropertyMetadata(null));
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private void SafeNavigate(object parameter)
        {
            if (parameter is string uri && Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            {
                Process.Start(uri);
            }
        }
    }
}
