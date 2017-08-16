# Navigation Pane

The navigation pane project type includes a navigation menu displayed in a panel at the side of the screen and which can be expanded with the Hamburger icon. The menu can be modified in the following ways.

* Change the icon for an item in the navigation panel menu
* Change the text for an item in the navigation panel menu

## Change the icon for an item

By default every item in the navigation pane is displayed with the symbol for a document.
When every item has the same icon it is hard to differentiate between them when the navigation panel is collapsed. In almost all cases you will want to change the icon used.

![](../resources/modifications/NavMenu_Different_Symbols.png)

Navigate to `ViewModel/ShellViewModel.cs` (or `Views/ShellPage.xaml.cs` if using Code Behind) and change the `PopulateNavItems` method.

The code below shows the symbols used to create the app shown in the image above.

```csharp
private void PopulateNavItems()
{
    _primaryItems.Clear();
    _secondaryItems.Clear();

    _primaryItems.Add(ShellNavigationItem.FromType<MainPage>("Shell_Main".GetLocalized(), Symbol.Home));
    _primaryItems.Add(ShellNavigationItem.FromType<MapPage>("Shell_Map".GetLocalized(), Symbol.Map));
    _primaryItems.Add(ShellNavigationItem.FromType<MasterDetailPage>("Shell_MasterDetail".GetLocalized(), Symbol.DockLeft));
    _primaryItems.Add(ShellNavigationItem.FromType<TabbedPage>("Shell_Tabbed".GetLocalized(), Symbol.Document)); // This is still the default
    _primaryItems.Add(ShellNavigationItem.FromType<WebViewPage>("Shell_WebView".GetLocalized(), Symbol.Globe));
    _secondaryItems.Add(ShellNavigationItem.FromType<SettingsPage>("Shell_Settings".GetLocalized(), Symbol.Setting));
}
```

The icons are created using the `Windows.UI.Xaml.Controls.Symbol` enumeration. You can view all the symbols available at <https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.symbol>

You can also set the menu item to use an `IconElement` directly. Like this:

```csharp
_navigationItems.Add(ShellNavigationItem.FromType<MainView>("Shell_Main".GetLocalized(), new FontIcon { Glyph = "\uED5A" }));
```

## Change the text for an item

The text for a shell navigation item comes from the localized string resources. For an item which defines the text with `"Shell_Main".GetLocalized()` the value "Shell_Main" corresponds with an entry in `Resources.resw`. Change the value in the resources file to alter what is displayed in the navigation menu.