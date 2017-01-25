using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Page_NS.BasicSettingsPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BasicSettingsPagePage : Page
    {
        public BasicSettingsPagePage()
        {
            this.InitializeComponent();            
            ViewModel = new BasicSettingsPageViewModel();
            DataContext = ViewModel;
        }
        public BasicSettingsPageViewModel ViewModel { get; private set; }
    }
}
