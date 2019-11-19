# Menu Bar

This project contains a top menu bar with File and Views menu entries and a Blank canvas to show your views. The project includes navigation methods to show views five different ways:

- Update view
- Navigate
- Open in right pane
- Open in a new Window
- Open in a dialog

 Learn more about using top-level menus in apps UWP apps [here](https://docs.microsoft.com/windows/uwp/design/controls-and-patterns/menus#create-a-menu-bar).

This document covers:

- [Use different ways to show pages](#navigation)
- [Add new menu items](#newmenuitems)

<a name="navigation"></a>

## Use different ways to show pages

The Menu bar project contains a MenuNavigationHelper (IMenuNavigationService in Prism Framework) class that provides methods to show pages in five different ways.

### 1.  Update view

Shows a page in the frame below the menu without the possibility to navigate back to previous pages. This is the default option in all pages except the settings page.

### 2. Navigate

Navigates to a page in the frame below the menu with the possibility to navigate back to previous pages. This option is used in pages with master and detail like ImageGallery and ContentGrid, where detail pages must add a back navigation button.

### 3. Open in the right pane

Shows a page in the right pane of the SplitView contained in the ShellPage. This is the default option in the settings page.

### 4. Open in a new window

Shows a page in a separate Window. This is done with the help of the MultiView feature.

### 5. Open in a dialog

Shows a page in a dialog over the app window with a button on the bottom to dismiss the dialog.

<a name="newmenuitems"></a>

## Add new MenuBar and MenuFlyout items

### Generated code

The menu bar project adds all your pages to the views menu entry by default (except the settings page, that is added under the file menu entry).

**ShellPage.xaml**

```xml
<winui:MenuBar VerticalAlignment="Top">
    <winui:MenuBarItem x:Uid="ShellMenuBarItem_File">
        <!-- Settings page is added here -->
        <MenuFlyoutItem x:Uid="ShellMenuItem_File_Exit" Click="ShellMenuItemClick_File_Exit" />
    </winui:MenuBarItem>
    <winui:MenuBarItem x:Uid="ShellMenuBarItem_Views">
        <!-- All other pages except settings page are added here -->
    </winui:MenuBarItem>
</winui:MenuBar>
```

### Add new items

The following code snippets show how to add a new menu "Help" with a menu entry "About" :

**ShellPage.xaml**

```xml
<winui:MenuBar VerticalAlignment="Top">
    <winui:MenuBarItem x:Uid="ShellMenuBarItem_File">
         <!-- Settings page is added here -->
        <MenuFlyoutItem x:Uid="ShellMenuItem_File_Exit" Click="ShellMenuItemClick_File_Exit" />
    </winui:MenuBarItem>
    <winui:MenuBarItem x:Uid="ShellMenuBarItem_Views">
         <!-- All other pages except settings page are added here -->
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