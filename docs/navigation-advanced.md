# Advanced App navigation
This documentation describes the steps to modify the NavigationService to contain different frames and different navigation levels. You can check out the sample code [here](https://github.com/Microsoft/WindowsTemplateStudio/tree/dev/poc/AdvancedNavigationPaneProject).
These are the scenarios shown in the App:

**App launching**
 - The App starts in a Startup page that only contains a start button.
 - By clicking on Start button the App Navigates to a NavigationPane Shell without the possibility to come back.

**NavigationPane Shell**
 - The navigation pane contains the main page and the settings page and you can navigate on that pages on the Secondary frame.
 - WebSite option in the pane footer allows you to navigate to a new web page in the main frame.
 - SecondShell option in the pane footer allows you to navigate to a secondary navigation pane shell in the main frame.
 - LogOut option in the pane footer allows you to restart the application navigation stack and set the startup page after restarting all the navigation.

 **Second NavigationPane Shell**
 - Contains two pages (SecondMain & SecondOther) in which you can navigate in a new third frame.
 - WebSite option in the pane footer allows you to navigate to a new web page in the main frame.

 This App handles page navigations in three different frames and the NavigationService is tracking all the navigation stack in different Frames to allow you to come back across all the navigation tree.

## NavigationService