using ItemNamespace.ViewModel;
namespace ItemNamespace.View
{
    public sealed partial class uct.ItemNamePage : Page
    {    
        public uct.ItemNameViewModel ViewModel { get; } = new uct.ItemNameViewModel();

        public uct.ItemNamePage()
        {
            DataContext = ViewModel;
            this.InitializeComponent();
        }
    }
}
