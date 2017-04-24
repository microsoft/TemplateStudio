using Microsoft.Templates.UI.ViewModels;
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
using System.Windows.Threading;

namespace Microsoft.Templates.UI.Controls
{
    public enum StatusType { Empty, Information, Warning, Error }
    public partial class StatusControl : UserControl
    {
        public static StatusViewModel EmptyStatus = new StatusViewModel(StatusType.Empty, String.Empty);

        public StatusViewModel Status
        {
            get { return (StatusViewModel)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }
        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(StatusViewModel), typeof(StatusControl), new PropertyMetadata(new StatusViewModel(StatusType.Empty, String.Empty), OnStatusPropertyChanged));

        private static void OnStatusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as StatusControl;
            control.UpdateStatus(e.NewValue as StatusViewModel);
        }

        private DispatcherTimer _hideTimer;

        public StatusControl()
        {
            InitializeComponent();
            _hideTimer = new DispatcherTimer();
            _hideTimer.Interval = TimeSpan.FromSeconds(5);
            _hideTimer.Tick += OnHideTick;
        }

        private void OnHideTick(object sender, EventArgs e)
        {
            _hideTimer.Stop();
            UpdateStatus(EmptyStatus);
        }

        private void UpdateStatus(StatusViewModel status)
        {
            txtStatus.Text = status.Message;
            switch (status.Status)
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
            if (status.AutoHide == true)
            {
                _hideTimer.Start();
            }
        }
    }
}
