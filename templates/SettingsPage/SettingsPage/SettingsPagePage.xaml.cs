using Windows.UI.Xaml.Controls;

// The Settings Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Param_PageNS.SettingsPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPagePage : Page
    {
        public SettingsPagePage()
        {
            this.InitializeComponent();
#if (isBasic)
            ViewModel = new SettingsPageViewModel();
            DataContext = ViewModel;
#endif
        }
#if (isBasic)
        public SettingsPageViewModel ViewModel { get; private set; }
#endif
    }
}
