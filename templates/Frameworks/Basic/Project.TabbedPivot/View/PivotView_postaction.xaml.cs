public sealed partial class PivotView : Page
{
    public PivotViewModel ViewModel { get; } = new PivotViewModel();

    public PivotView()
    {
        DataContext = ViewModel;
        this.InitializeComponent();
    }
}
