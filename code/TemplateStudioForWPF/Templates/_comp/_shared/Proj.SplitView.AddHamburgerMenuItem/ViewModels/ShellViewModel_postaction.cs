﻿public ObservableCollection<HamburgerMenuItem> MenuItems { get; } = new ObservableCollection<HamburgerMenuItem>()
{
//^^
//{[{
    new HamburgerMenuGlyphItem() { Label = Resources.Shellts.ItemNamePage, Glyph = "\uE8A5", TargetPageType = typeof(ts.ItemNameViewModel) },
//}]}
};
