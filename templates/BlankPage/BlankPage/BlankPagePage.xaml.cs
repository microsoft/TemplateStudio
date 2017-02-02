using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Param_PageNS.BlankPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPagePage : Page
    {
        public BlankPagePage()
        {
            this.InitializeComponent();
#if (isBasic)
            ViewModel = new BlankPageViewModel();
            DataContext = ViewModel;
#endif
        }
#if (isBasic)
        public BlankPageViewModel ViewModel { get; private set; }
#endif
    }
}
