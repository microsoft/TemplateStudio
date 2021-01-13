*Recommended Markdown viewer: [Markdown Editor VS Extension](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.MarkdownEditor).*

This project was created using [Microsoft Windows Template Studio](https://aka.ms/wts).

## Getting Started
Windows UI Library (WinUI) 3 is a native user experience (UX) framework for both Windows Desktop and UWP apps.

This is a good time to build, deploy, and launch your app hitting F5. You can find the app entry point in `App.xaml.cs`, try adding a breakpoint in `OnLaunched` method and debugging the code, stepping into the `ActivationService` methods to understand the app lifecycle.

Don't forget to review the `developer TODOs` we've added for you. You can open the Task List using the menu `Views -> Task List`.

## File Structure
```
.
â”œâ”€â”€ Param_ProjectName/ - WinUI 3 Desktop app
â”‚ â”œâ”€â”€ Activation/ - app activation handlers
â”‚ â”œâ”€â”€ Behaviors/ - UI controls behaviors
â”‚ â”œâ”€â”€ Contracts/ - class interfaces
â”‚ â”œâ”€â”€ Helpers/ - static helper classes
â”‚ â”œâ”€â”€ Services/ - services implementations
â”‚ â”‚ â”œâ”€â”€ ActivationService.cs - app activation and initialization
â”‚ â”‚ â”œâ”€â”€ NavigationService.cs - navigate between pages
â”‚ â”‚ â””â”€â”€ ...
â”‚ â”œâ”€â”€ Strings/en-us/Resources.resw - localized string resources
â”‚ â”œâ”€â”€ Styles/ - custom style definitions
â”‚ â”œâ”€â”€ ViewModels/ - properties and commands consumed in the views
â”‚ â”œâ”€â”€ Views/ - UI pages
â”‚ â”‚ â”œâ”€â”€ Shell.xaml - main app window with navigation frame
â”‚ â”‚ â””â”€â”€ ...
â”‚ â””â”€â”€ App.xaml - app definition and lifecycle events
â”œâ”€â”€ Param_ProjectName.Core/ - core project (.NET Standard)
â”‚ â”œâ”€â”€ Contracts/ - class interfaces
â”‚ â”œâ”€â”€ Helpers/ - static helper classes
â”‚ â”œâ”€â”€ Models/ - business models
â”‚ â””â”€â”€ Services/ - services implementations
â”œâ”€â”€ Param_ProjectName (Package)/ - MSIX packaging project
â”‚ â”œâ”€â”€ Images/ - images for MSIX app logos
â”‚ â”œâ”€â”€ TemporaryKey.pfx - test certificate
â”‚ â”œâ”€â”€ Strings/en-us/Resources.resw - localized string resources
â”‚ â””â”€â”€ Package.appxmanifest - app properties and declarations
â””â”€â”€ README.md
```

### Design pattern
This app uses MVVM Toolkit, for more information see https://aka.ms/mvvmtoolkit.

### Project type
This app uses Navigation Pane, for more information see [navigation pane docs](https://github.com/microsoft/WindowsTemplateStudio/blob/dev/docs/UWP/projectTypes/navigationpane.md).

## Publish / Distribute

Use the [packaging project](http://aka.ms/msix) to create the app package to distribute your app and future updates. Right click on the packaging project and click `Publish -> Create App Packages...` to create an app package.

## Additional Documentation

- [WTS WinUI 3 docs](https://github.com/microsoft/WindowsTemplateStudio/tree/dev/docs/WinUI)
- [WinUI 3 docs](https://docs.microsoft.com/windows/apps/winui/winui3/)
- [WinUI 3 GitHub repo](https://github.com/microsoft/microsoft-ui-xaml)
