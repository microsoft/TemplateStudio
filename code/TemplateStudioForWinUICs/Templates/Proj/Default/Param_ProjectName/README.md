*Recommended Markdown viewer: [Markdown Editor VS Extension](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.MarkdownEditor2).*

This project was created using [Microsoft Template Studio](https://aka.ms/templatestudio).

## Getting Started
This app was built using WinUI 3 and the Windows App SDK.
Windows UI Library (WinUI) 3 is a native user experience (UX) framework for Windows Desktop apps.

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
│ │ ├── ShellPage.xaml - main app page with navigation frame (only for NavView and MenuBar)
│ │ └── ...
│ └── App.xaml - app definition and lifecycle events
│ └── Package.appxmanifest - app properties and declarations
├── Param_ProjectName.Core/ - core project (.NET Standard)
│ ├── Contracts/ - class interfaces
│ ├── Helpers/ - static helper classes
│ ├── Models/ - business models
│ └── Services/ - services implementations
└── README.md
```

### Design pattern

### Project type

## Publish / Distribute
For projects with MSIX packaging enabled, right-click on the application project and select `Package and Publish -> Create App Packages...` to create an MSIX package.

## Additional Documentation

- [TS WinUI 3 docs](https://github.com/microsoft/TemplateStudio/tree/main/docs/WinUI)
- [WinUI 3 docs](https://docs.microsoft.com/windows/apps/winui/winui3/)
- [WinUI 3 GitHub repo](https://github.com/microsoft/microsoft-ui-xaml)
- [Windows App SDK GitHub repo](https://github.com/microsoft/WindowsAppSDK)
