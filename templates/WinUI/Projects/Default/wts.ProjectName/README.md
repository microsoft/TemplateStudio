*Recommended Markdown viewer: [Markdown Editor VS Extension](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.MarkdownEditor).*

This project was created using [Microsoft Windows Template Studio](https://aka.ms/wts).

## Getting Started
This app was built using WinUI 3 and Project Reunion 0.5.
Windows UI Library (WinUI) 3 is a native user experience (UX) framework for both Windows Desktop and UWP apps.

You're ready to build, deploy, and launch your app hitting F5. You can find the app entry point in the `App.xaml.cs` file. 
Add a breakpoint in the `OnLaunched` method and debug the code. Step into the `ActivationService` methods to understand the app lifecycle.

Don't forget to review the `developer TODOs` we've added for you. 
You can open the Task List using the menu `Views -> Task List`.

## File Structure
```
.
├── Param_ProjectName/ - WinUI 3 Desktop app
│ ├── Activation/ - app activation handlers
│ ├── Behaviors/ - UI controls behaviors
│ ├── Contracts/ - class interfaces
│ ├── Helpers/ - static helper classes
│ ├── Services/ - services implementations
│ │ ├── ActivationService.cs - app activation and initialization
│ │ ├── NavigationService.cs - navigate between pages
│ │ └──  ...
│ ├── Strings/en-us/Resources.resw - localized string resources
│ ├── Styles/ - custom style definitions
│ ├── ViewModels/ - properties and commands consumed in the views
│ ├── Views/ - UI pages
│ │ ├── ShellPage.xaml - main app page with navigation frame (only for SplitView and MenuBar)
│ │ └── ...
│ └── App.xaml - app definition and lifecycle events
├── Param_ProjectName.Core/ - core project (.NET Standard)
│ ├── Contracts/ - class interfaces
│ ├── Helpers/ - static helper classes
│ ├── Models/ - business models
│ └── Services/ - services implementations
├── Param_ProjectName (Package)/ - MSIX packaging project
│ ├── Strings/en-us/Resources.resw - localized string resources
│ └── Package.appxmanifest - app properties and declarations
└── README.md
```

### Design pattern

### Project type

## Publish / Distribute

Use the [packaging project](http://aka.ms/msix) to create the app package to distribute your app and future updates. 
Right click on the packaging project and click `Publish -> Create App Packages...` to create an app package.

## Additional Documentation

- [WTS WinUI 3 docs](https://github.com/microsoft/WindowsTemplateStudio/tree/dev/docs/WinUI)
- [WinUI 3 docs](https://docs.microsoft.com/windows/apps/winui/winui3/)
- [WinUI 3 GitHub repo](https://github.com/microsoft/microsoft-ui-xaml)
- [Project Reunion GitHub repo](https://github.com/microsoft/ProjectReunion)
