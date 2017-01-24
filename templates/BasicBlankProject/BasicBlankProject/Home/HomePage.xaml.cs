using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BasicBlankProject.Home
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
            ViewModel = new HomeViewModel();
            DataContext = ViewModel;
        }
        public HomeViewModel ViewModel { get; private set; }
    }
}
