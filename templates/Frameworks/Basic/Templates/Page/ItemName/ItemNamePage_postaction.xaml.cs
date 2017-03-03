public sealed partial class ItemNamePage : Page
{
    public ItemNameViewModel ViewModel { get; }

    public ItemNamePage()
    {
        ViewModel = new ItemNameViewModel();
        DataContext = ViewModel;

        this.InitializeComponent();
    }
}
