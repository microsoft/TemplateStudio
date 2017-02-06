using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Param_PageNS.MasterDetailPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MasterDetailPagePage : Page
    {
        public MasterDetailPagePage()
        {
            this.InitializeComponent();
#if (isBasic)
            ViewModel = new MasterDetailPageViewModel();
            DataContext = ViewModel;
#endif
        }
#if (isBasic)
        public MasterDetailPageViewModel ViewModel { get; private set; }
#endif
    }
}
