# MenuBar

The project includes a menu bar on top of the screen that gives access to the pages of the application.

Menu Bars are used a lot in desktop applications like Outlook, Word or Visual Studio.

The menu initially shows two entries, File and Views. Pages are added to the Views menu entry, Settings Page to the file entry. Once the project is created you can redistribute or create new menu entries as convenient.

This project contains a top menu bar with File and Views menu entries and a Blank canvas to show your views. The project includes navigation methods to show views five different ways:

- Update view
- Navigate
- Open in right pane

Learn more about using top-level menus in apps WinUI apps [here](https://docs.microsoft.com/windows/winui/api/microsoft.ui.xaml.controls.menubar?view=winui-3.0).

This document covers:

- [Use different ways to show pages](#navigation)
- [Add new menu items](#newmenuitems)

<a name="navigation"></a>

## Use different ways to show pages

The Menu bar project contains services that provides methods to show pages in five different ways.

### 1.  Update view

Shows a page in the frame below the menu without the possibility to navigate back to previous pages. This is the default option in all pages except the settings page. You should use the NavigateTo method in the INavigationService class passing the clearNavigation parameter as true.

### 2. Navigate

Navigates to a page in the frame below the menu with the possibility to navigate back to previous pages.

### 3. Open in the right pane

Shows a page in the right pane of the SplitView contained in the ShellWindow. This is the default option in the settings page. You should use the OpenInRightPane method in the IRightPaneService class.

<a name="newmenuitems"></a>

## Add new MenuBar and MenuFlyout items

### Generated code

The menu bar project adds all your pages to the views menu entry by default (except the settings page, that is added under the file menu entry).

**ShellPage.xaml**

```xml
<MenuBar VerticalAlignment="Top">
    <MenuBar.Items>
        <MenuBarItem x:Uid="ShellMenuBarItem_File">
            <MenuBarItem.Items>
                <!-- Settings page is added here -->
                <MenuFlyoutItem x:Uid="ShellMenuItem_File_Exit" Command="{x:Bind ViewModel.MenuFileExitCommand}" />
            </MenuBarItem.Items>
        </MenuBarItem>
        <MenuBarItem x:Uid="ShellMenuBarItem_Views">
            <MenuBarItem.Items>
                <!-- All other pages except settings page are added here -->
            </MenuBarItem.Items>
        </MenuBarItem>
    </MenuBar.Items>
</MenuBar>
```

### Add new items

The following code snippets show how to add a new menu "Help" with a menu entry "About" :

**ShellPage.xaml**

```xml
<MenuBar VerticalAlignment="Top">
    <MenuBar.Items>
        <MenuBarItem x:Uid="ShellMenuBarItem_File">
            <MenuBarItem.Items>
                <!-- Settings page is added here -->
                <MenuFlyoutItem x:Uid="ShellMenuItem_File_Exit" Command="{x:Bind ViewModel.MenuFileExitCommand}" />
            </MenuBarItem.Items>
        </MenuBarItem>
        <MenuBarItem x:Uid="ShellMenuBarItem_Views">
            <MenuBarItem.Items>
                <!-- All other pages except settings page are added here -->
            </MenuBarItem.Items>
        </MenuBarItem>

        <!-- Add this block -->
        <MenuBarItem x:Uid="ShellMenuBarItem_Help">
            <MenuFlyoutItem x:Uid="ShellMenuItem_Help_About" Command="{x:Bind ViewModel.MenuHelpAboutCommand}" />
        </MenuBarItem>
        <!-- End of block -->
    </MenuBar.Items>
</MenuBar>
```

**Resources.resw**

```xml
<!-- Add this block -->
<data name="ShellMenuBarItem_Help.Title" xml:space="preserve">
    <value>Help</value>
    <comment>Menu bar item Title for Help</comment>
</data>
<data name="ShellMenuItem_Help_About.Text" xml:space="preserve">
    <value>About</value>
    <comment>Menu item text for About</comment>
</data>
<!-- End of block -->
```

**ShellViewModel.cs**

```csharp
public class ShellViewModel : ObservableRecipient
{
    // Add this block
    private ICommand _menuHelpAboutCommand;

    public ICommand MenuHelpAboutCommand => _menuHelpAboutCommand ?? (_menuHelpAboutCommand = new RelayCommand(OnMenuHelpAbout));

    private void OnMenuHelpAbout()
    {
        // Run here the code for Help -> About menu item
    }
    // End of block
}
```