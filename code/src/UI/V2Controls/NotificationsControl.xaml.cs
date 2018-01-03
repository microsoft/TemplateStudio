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

namespace Microsoft.Templates.UI.V2Controls
{
    /// <summary>
    /// Interaction logic for NotificationsControl.xaml
    /// </summary>
    public partial class NotificationsControl : UserControl
    {
        public static NotificationsControl Instance;

        public Notification Notification
        {
            get => (Notification)GetValue(NotificationProperty);
            set => SetValue(NotificationProperty, value);
        }

        public static readonly DependencyProperty NotificationProperty = DependencyProperty.Register("Notification", typeof(Notification), typeof(NotificationsControl), new PropertyMetadata(null));

        public NotificationsControl()
        {
            Instance = this;
            InitializeComponent();
        }

        public void AddNotification(Notification notification)
        {
            Notification = notification;
        }
    }
}
