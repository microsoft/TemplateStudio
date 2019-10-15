namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : System.ComponentModel.INotifyPropertyChanged, IDisposable
    {
        private HamburgerMenuItem _selectedMenuItem;
//{[{
        private HamburgerMenuItem _selectedOptionsMenuItem;
//}]}
        private ICommand _menuItemInvokedCommand;
//{[{
        private ICommand _optionsMenuItemInvokedCommand;
//}]}

        public HamburgerMenuItem SelectedMenuItem
        {
            get { return _selectedMenuItem; }
            set { Set(ref _selectedMenuItem, value); }
        }
//{[{

        public HamburgerMenuItem SelectedOptionsMenuItem
        {
            get { return _selectedOptionsMenuItem; }
            set { Set(ref _selectedOptionsMenuItem, value); }
        }
//}]}
        public ObservableCollection<HamburgerMenuItem> MenuItems { get; } = new ObservableCollection<HamburgerMenuItem>()
        {
        };
//{[{

        public ObservableCollection<HamburgerMenuItem> OptionMenuItems { get; } = new ObservableCollection<HamburgerMenuItem>()
        {

            new HamburgerMenuGlyphItem() { Label = Resources.Shellwts.ItemNamePage, Glyph = "\uE713", TargetPageType = typeof(wts.ItemNameViewModel) }
        };
//}]}

        public ICommand MenuItemInvokedCommand => _menuItemInvokedCommand ?? (_menuItemInvokedCommand = new System.Windows.Input.ICommand(MenuItemInvoked));
//{[{

        public ICommand OptionsMenuItemInvokedCommand => _optionsMenuItemInvokedCommand ?? (_optionsMenuItemInvokedCommand = new System.Windows.Input.ICommand(OptionsMenuItemInvoked));
//}]}

//^^
//{[{
        private void OptionsMenuItemInvoked()
            => _navigationService.NavigateMethodName(SelectedOptionsMenuItem.TargetPageType.FullName);
//}]}
        private void OnNavigated(object sender, string viewModelName)
        {
            var item = MenuItems
                        .OfType<HamburgerMenuItem>()
                        .FirstOrDefault(i => viewModelName == i.TargetPageType.FullName);
            if (item != null)
            {
                SelectedMenuItem = item;
            }
//{[{
            else
            {
                SelectedOptionsMenuItem = OptionMenuItems
                        .OfType<HamburgerMenuItem>()
                        .FirstOrDefault(i => viewModelName == i.TargetPageType.FullName);
            }
//}]}
        }
    }
}
