# MenuBar

This project contains a top menu bar with File and Views options and a Blank canvas to show your views. The project includes navigation methods to show views in different ways. Learn more about using top-level menus in apps UWP apps [here](https://docs.microsoft.com/windows/uwp/design/controls-and-patterns/menus#create-a-menu-bar).

This document covers:

* [Use different ways to show pages](#navigation)
* [Add new menu items](#newmenuitems)

<a name="navigation"></a>

## Use different ways to show pages
Menu bar projects contain a MenuNavigationHelper or IMenuNavigationService in Prism Framework classes that provides methods to show pages in different ways.

### Update view
Show a page in the frame below of the menu without the possibility to navigate back to previous pages. This is the default option in all pages except the settings page.

### Navigate
Navigate into a page in the frame below of the menu with the possibility to navigate back to previous pages. This option is used in pages with master and detail like ImageGallery and ContentGrid, detail pages must add a back navigation button.

### Open in the right pane
Show a page in the right pane of the SplitView located at ShellPage. This is the default option in the settings page.

### Open in a new window
MenuBar projects adds the MultiView feature in WTS to allow MenuNavigationHelper show pages in separated windows.

### Open in a dialog
Show a page in a dialog over the app window with a button on the bottom to dismiss the dialog.

<a name="newmenuitems"></a>

## Add new MenuBar and MenuFlyout items

### Generated code
This is de MenuBar implementation.

**ShellPage.xaml**
```xml
<winui:MenuBar VerticalAlignment="Top">
    <winui:MenuBarItem x:Uid="ShellMenuBarItem_File">
        <!-- Here settings page -->
        <MenuFlyoutItem x:Uid="ShellMenuItem_File_Exit" Click="ShellMenuItemClick_File_Exit" />
    </winui:MenuBarItem>
    <winui:MenuBarItem x:Uid="ShellMenuBarItem_Views">
        <!-- Here all your pages except settings page -->
    </winui:MenuBarItem>
</winui:MenuBar>
```

### Add new items

This is the way to add a new MenuBarItem with new MenuFlyoutItems (i.e. Help > About).

**ShellPage.xaml**
```xml
<winui:MenuBar VerticalAlignment="Top">
    <winui:MenuBarItem x:Uid="ShellMenuBarItem_File">
        <!-- Here settings page -->
        <MenuFlyoutItem x:Uid="ShellMenuItem_File_Exit" Click="ShellMenuItemClick_File_Exit" />
    </winui:MenuBarItem>
    <winui:MenuBarItem x:Uid="ShellMenuBarItem_Views">
        <!-- Here all your pages except settings page -->
    </winui:MenuBarItem>

    <!-- Add this block -->
    <winui:MenuBarItem x:Uid="ShellMenuBarItem_Help">
        <MenuFlyoutItem x:Uid="ShellMenuItem_Help_About" Click="ShellMenuItemClick_Help_About" />
    </winui:MenuBarItem>
    <!-- End of block -->

</winui:MenuBar>
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
<a name="invokecode"></a>