public ObservableCollection<HamburgerMenuItem> MenuItems { get; } = new ObservableCollection<HamburgerMenuItem>()
{
//^^
//{[{
    new HamburgerMenuGlyphItem() { Label = Resources.Shellts.ItemNamePage, Glyph = "\uE8A5", Tag = PageKeys.ts.ItemName },
//}]}
};
