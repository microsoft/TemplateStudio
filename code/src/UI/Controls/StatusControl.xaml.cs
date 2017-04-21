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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Microsoft.Templates.UI.Controls
{
    public enum StatusType { Empty, Information, Warning, Error }
    public partial class StatusControl : UserControl
    {
        public static (StatusType, string) EmptyStatus = (StatusType.Empty, String.Empty);

        public (StatusType StatusType, string StatusMessage) Status
        {
            get { return ((StatusType StatusType, string StatusMessage))GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }
        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof((StatusType StatusType, string StatusMessage)), typeof(StatusControl), new PropertyMetadata((StatusType.Empty, String.Empty), OnStatusPropertyChanged));

        private static void OnStatusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as StatusControl;
            control.UpdateStatus();
        }

        public StatusControl()
        {
            InitializeComponent();
        }

        private void UpdateStatus()
        {
            txtStatus.Text = Status.StatusMessage;
            switch (Status.StatusType)
            {
                case StatusType.Information:
                    txtIcon.Text = Char.ConvertFromUtf32(0xE930);
                    txtIcon.Foreground = FindResource("UIBlack") as SolidColorBrush;
                    this.Background = new SolidColorBrush(Colors.Transparent);
                    break;
                case StatusType.Warning:
                    txtIcon.Text = Char.ConvertFromUtf32(0xEA39);
                    txtIcon.Foreground = FindResource("UIDarkYellow") as SolidColorBrush;
                    //this.Background = FindResource("UIYellow") as SolidColorBrush;
                    Color yellow = (Color)FindResource("UIYellowColor");
                    this.Background = new LinearGradientBrush(yellow, Colors.Transparent, 0);
                    break;
                case StatusType.Error:
                    txtIcon.Text = Char.ConvertFromUtf32(0xEB90);
                    txtIcon.Foreground = FindResource("UIRed") as SolidColorBrush;
                    this.Background = new SolidColorBrush(Colors.Transparent);
                    break;
                default:
                    txtIcon.Text = String.Empty;
                    this.Background = new SolidColorBrush(Colors.Transparent);
                    break;
            }
        }
    }
}
