# Menu bar project type

The project includes a menu bar on top of the screen that gives access to the pages of the application.

Menu Bars are used a lot in desktop applications like Outlook, Word or Visual Studio.

The menu initially shows two entries, File and Views. Pages are added to the Views menu entry, Settings Page to the file entry. Once the project is created you can redistribute or create new menu entries as convenient.

This project contains a top menu bar with File and Views menu entries and a Blank canvas to show your views. The project includes navigation methods to show views five different ways:

- Update view
- Navigate
- Open in right pane
- Open in a new Window
- Open in a dialog

This document covers:

- [Use different ways to show pages](#navigation)
- [Add new menu items](#newmenuitems)

<a name="navigation"></a>

## Use different ways to show pages

The Menu bar project contains services that provides methods to show pages in five different ways.

### 1.  Update view

Shows a page in the frame below the menu without the possibility to navigate back to previous pages. This is the default option in all pages except the settings page. You should use the NavigateTo method in the NavigationService class passing the clearNavigation parameter as true.

### 2. Navigate

Navigates to a page in the frame below the menu with the possibility to navigate back to previous pages.

### 3. Open in the right pane

Shows a page in the right pane of the SplitView contained in the ShellWindow. This is the default option in the settings page. You should use the OpenInRightPane method in the RightPaneService class.

### 4. Open in a new window

Shows a page in a separate Window. This is done with the help of the MultiView feature. You should use the OpenInNewWindow method in the WindowManagerService class. (Except in Prism where you should use the Prism dialog service.) You should add a dependency injection to this service in the ShellViewModel constructor.

### 5. Open in a dialog

Shows a page in a dialog over the app window with a button on the bottom to dismiss the dialog. You should use the OpenInDialog method in the WindowManagerService class. (Except in Prism where you should use the Prism DialogService.) You should add a dependency injection to this service in the ShellViewModel constructor.

<a name="newmenuitems"></a>

## Add new Menu and MenuItem items

### Generated code

The menu bar project adds all your pages to the views menu entry by default (except the settings page, that is added under the file menu entry).

**ShellWindow.xaml**

```xml
<Menu>
    <MenuItem Header="{x:Static strings:Resources.ShellMenuFileHeader}">
        <!-- Settings page is added here -->
        <Separator />
        <MenuItem Header="{x:Static strings:Resources.ShellMenuItemFileExitHeader}" Command="{Binding MenuFileExitCommand}" />
    </MenuItem>
    <MenuItem Header="{x:Static strings:Resources.ShellMenuViewsHeader}">
        <!-- All other pages except settings page are added here -->
    </MenuItem>
</Menu>
```

### Add new items

The following code snippets show how to add a new menu "Help" with a menu entry "About" :

**ShellWindow .xaml**

```xml
<Menu>
    <MenuItem Header="{x:Static strings:Resources.ShellMenuFileHeader}">
        <!-- Settings page is added here -->
        <Separator />
        <MenuItem Header="{x:Static strings:Resources.ShellMenuItemFileExitHeader}" Command="{Binding MenuFileExitCommand}" />
    </MenuItem>
    <MenuItem Header="{x:Static strings:Resources.ShellMenuViewsHeader}">
        <!-- All other pages except settings page are added here -->
    </MenuItem>

    <!-- Add this block -->
    <MenuItem Header="{x:Static strings:Resources.ShellMenuHelpHeader}">
        <MenuItem Header="{x:Static strings:Resources.ShellMenuItemHelpAboutHeader}" Command="{Binding MenuHelpAboutCommand}" />
    </MenuItem>
</Menu>
```

**Resources.resw**

```xml
<!-- Add this block -->
<data name="ShellMenuHelpHeader" xml:space="preserve">
    <value>Help</value>
    <comment>Menu bar item Title for Help</comment>
</data>
<data name="ShellMenuItemHelpAboutHeader" xml:space="preserve">
    <value>About</value>
    <comment>Menu item text for About</comment>
</data>
<!-- End of block -->
```



Menu bar projects use [MahApps.Metro](../mahapps-metro.md) to add modern styles to the user interface.