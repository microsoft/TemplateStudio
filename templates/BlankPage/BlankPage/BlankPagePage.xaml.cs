using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Page_NS.BlankPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPagePage : Page
    {
        public BlankPagePage()
        {
            this.InitializeComponent();
            ViewModel = new BlankPageViewModel();
            DataContext = ViewModel;
        }
        public BlankPageViewModel ViewModel { get; private set; }
    }
}
