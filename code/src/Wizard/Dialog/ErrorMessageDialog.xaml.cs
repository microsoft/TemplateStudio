using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Microsoft.Templates.Wizard.Dialog
{

    public partial class ErrorMessageDialog : Window
    {
        private static ErrorMessageDialog dialog;
        private static string _title;
        private static string _message;
        private static string _exception;
        private static MessageBoxImage _image;

        public ErrorMessageDialog()
        {
            InitializeComponent();
            Title = _title;
            Message.Text = _message;
            ExceptionDetail.Text = _exception;
        }


        public static void Show(string message, string title, string exceptionDetail, MessageBoxImage image)
        {
            _title = title;
            _message = message;
            _exception = exceptionDetail;
            _image = image;
            dialog = new ErrorMessageDialog();
            dialog.ShowDialog();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void SetImage(MessageBoxImage image)
        {
            //TODO: Set image in the form
        }
    }
}
