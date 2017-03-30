public sealed partial class PivotPage : Page
{
    public PivotViewModel ViewModel { get; } = new PivotViewModel();

    public PivotPage()
    {
        DataContext = ViewModel;
        this.InitializeComponent();
    }
}
