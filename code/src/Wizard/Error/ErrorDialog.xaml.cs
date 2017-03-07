using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Microsoft.Templates.Wizard.Error
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
