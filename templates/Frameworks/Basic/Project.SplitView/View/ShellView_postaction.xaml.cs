using RootNamespace.ViewModel;
namespace RootNamespace.View
{
    public sealed partial class ShellView : Page
    {
        public ShellViewModel ViewModel { get; } = new ShellViewModel();

        public ShellView()
        {
            DataContext = ViewModel;
            this.InitializeComponent();

            NavigationService.SetNavigationFrame(frame);
        }
    }
}
