using Microsoft.Templates.UI.ViewModels;
using System;
using System.Windows;

namespace Microsoft.Templates.UI.Error
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
