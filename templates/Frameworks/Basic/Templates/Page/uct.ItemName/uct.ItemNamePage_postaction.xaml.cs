public sealed partial class uct.ItemNamePage : Page
{
    public uct.ItemNameViewModel ViewModel { get; }

    public uct.ItemNamePage()
    {
        ViewModel = new uct.ItemNameViewModel();
        DataContext = ViewModel;

        this.InitializeComponent();
    }
}
