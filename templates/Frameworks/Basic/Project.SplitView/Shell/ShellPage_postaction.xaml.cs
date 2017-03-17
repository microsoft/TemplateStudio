public sealed partial class ShellPage : Page
{
    public ShellViewModel ViewModel { get; } = new ShellViewModel();

    public ShellPage()
    {
        DataContext = ViewModel;
        this.InitializeComponent();

        NavigationService.SetNavigationFrame(frame);
    }
}