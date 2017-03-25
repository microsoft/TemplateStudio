using Microsoft.Practices.ServiceLocation;

using ItemNamespace.ViewModel;

namespace uct.ItemName.View
{
    public sealed partial class ShellView : Page
    {
        private ShellViewModel ViewModel { get { return DataContext as ShellViewModel; } }
    }
}