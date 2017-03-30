using RootNamespace.ViewModels;
namespace RootNamespace.Views
{
    public sealed partial class ShellPage : Page
    {
        public ShellViewModel ViewModel { get; } = new ShellViewModel();
    }
}
