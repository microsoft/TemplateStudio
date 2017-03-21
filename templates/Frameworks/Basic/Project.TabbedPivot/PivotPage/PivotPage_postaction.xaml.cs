public sealed partial class PivotPage : Page
{
    public PivotPageViewModel ViewModel { get; } = new PivotPageViewModel();

    public PivotPage()
    {
        DataContext = ViewModel;
        this.InitializeComponent();
    }
}
