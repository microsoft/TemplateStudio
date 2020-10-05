namespace Param_RootNamespace.Views
{
    public partial class ShellWindow : MetroWindow, IShellWindow, INotifyPropertyChanged
    {
        private HamburgerMenuItem _selectedMenuItem;
//{[{
        private HamburgerMenuItem _selectedOptionsMenuItem;
//}]}
        public HamburgerMenuItem SelectedMenuItem
        {
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
            new HamburgerMenuGlyphItem() { Label = Properties.Resources.Shellwts.ItemNamePage, Glyph = "\uE713", TargetPageType = typeof(wts.ItemNamePage) }
        };
//}]}
        public ShellWindow(INavigationService navigationService)
        {
        }
//^^
//{[{
        private void OnOptionsItemClick(object sender, ItemClickEventArgs args)
            => NavigateTo(SelectedOptionsMenuItem.TargetPageType);
//}]}

        private void NavigateTo(Type targetPage)
        {
        }

        private void OnNavigated(object sender, Type pageType)
        {
            var item = MenuItems
                        .OfType<HamburgerMenuItem>()
                        .FirstOrDefault(i => pageType == i.TargetPageType);
            if (item != null)
            {
            }
//{[{
            else
            {
                SelectedOptionsMenuItem = OptionMenuItems
                        .OfType<HamburgerMenuItem>()
                        .FirstOrDefault(i => pageType == i.TargetPageType);
            }
//}]}
        }
    }
}
