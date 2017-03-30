using ItemNamespace.ViewModels;
using Microsoft.Practices.ServiceLocation;
namespace uct.ItemName.Views
{
    public sealed partial class ShellPage : Page
    {
        private ShellViewModel ViewModel { get { return DataContext as ShellViewModel; } }
    }
}