# Navigation Pane

The navigation pane project type includes a navigation menu displayed in a panel at the side of the screen and which can be expanded with the Hamburger icon. The menu can be modified in the following ways.

* Change the icon for an item in the navigation panel menu
* Change the text for an item in the navigation panel menu

## Change the icon for an item

By default every item in the navigation pane is displayed with the symbol for a document.
When every item has the same icon it is hard to differentiate between them when the navigation panel is collapsed. In almost all cases you will want to change the icon used.

![](../resources/modifications/NavMenu_Different_Symbols.png)

Navigate to `ViewModel/ShellViewModel.cs` and change the `PopulateNavItems` method.

The code below shows the symbols used to create the app shown in the image above.

```csharp
private void PopulateNavItems()
{
    _navigationItems.Clear();

    _navigationItems.Add(ShellNavigationItem.FromType<MainView>("Shell_Main".GetLocalized(), Symbol.Home));
    _navigationItems.Add(ShellNavigationItem.FromType<MapView>("Shell_Map".GetLocalized(), Symbol.Map));
    _navigationItems.Add(ShellNavigationItem.FromType<MasterDetailView>("Shell_MasterDetail".GetLocalized(), Symbol.DockLeft));
    _navigationItems.Add(ShellNavigationItem.FromType<SettingsView>("Shell_Settings".GetLocalized(), Symbol.Setting));
    _navigationItems.Add(ShellNavigationItem.FromType<TabbedView>("Shell_Tabbed".GetLocalized(), Symbol.Document)); // This is still the default
    _navigationItems.Add(ShellNavigationItem.FromType<WebView1View>("Shell_WebView1".GetLocalized(), Symbol.Globe));
}
```

The icons are created using the `Windows.UI.Xaml.Controls.Symbol` enumeration. You can view all the symbols available at <https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.symbol>

## Change the text for an item

The text for a shell navigation item comes from the localized string resources. For an item which defines the text with `"Shell_Main".GetLocalized()` the value "Shell_Main" corresponds with an entry in `Resources.resw`. Change the value in the resources file to alter what is displayed in the navigation menu.