public ObservableCollection<HamburgerMenuItem> MenuItems { get; } = new ObservableCollection<HamburgerMenuItem>()
{
//^^
//{[{
    new HamburgerMenuGlyphItem() { Label = Resources.Shellwts.ItemNamePage, Glyph = "\uE8A5", TargetPageType = typeof(wts.ItemNameViewModel) },
//}]}
};
