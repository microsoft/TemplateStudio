public ObservableCollection<HamburgerMenuItem> MenuItems { get; } = new ObservableCollection<HamburgerMenuItem>()
{
//^^
//{[{
    new HamburgerMenuGlyphItem() { Label = Properties.Resources.Shellts.ItemNamePage, Glyph = "\uE8A5", TargetPageType = typeof(ts.ItemNamePage) },
//}]}
};
