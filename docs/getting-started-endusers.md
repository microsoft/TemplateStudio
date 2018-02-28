# Using and extending your generated project

Projects created with Windows Template Studio are intended as a starting point, and will require modification and extension before they're finished. This page explains common ways to extend what is generated for you.

## Understanding generated code from Windows Template Studio 
The final generated code is the result of a project configuration (project type and design pattern) and a multiple template choice (pages and features). There are a few concepts to understand before start working on the generated code.

- [Application activation](activation.md)
- [Navigation between pages](navigation.md)
- [Notifications in Windows Template Studio](notifications.md)

## Understanding concepts for Windows Template Studio

Windows Template Studio approaches UWP app creation using the following four attribute sets to decide how to best generate your app.  Below we will describe everything you can do.

### Project Types
**Project types** define the basic look and feel of your UWP app.

| Project type | Description |
|-------------:|:------------|
| Basic | This basic project is a blank canvas upon which to build your app. It provides no scaffolding and leaves everything up to you. |
| [Navigation Pane](projectTypes/navigationpane.md) | This project includes a navigation pane (or 'hamburger menu') at the side of the screen to enable easy user navigation between pages. This style is popular in mobile apps, but also works well on larger screens. The menu can be hidden when space is limited, or when it isn't needed.|
| Pivot and Tabs | Tabs across the top allow for quickly navigating between pages. The pivot control is useful for navigating between related or frequently accessed pages. The user can navigate between pivot panes (pages) by selecting from the text headers, which are always displayed.|

### Design patterns

**App Design patterns** define the coding pattern that will be used across the project, tying your UI and code together. Windows Template Studio currently supports the three most common patterns:

| Design pattern| Description |
|--------------:|:------------|
| Code Behind   | Code is coupled directly with a XAML file using a .xaml.cs extension. If you developed in WinForms and feel comfortable with that style of development, this is a great option for you. |
| MVVM Basic    | A generic implementation of the [Model-View-ViewModel (MVVM) pattern](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel), which can be used on all XAML platforms. Its intent is to provide a clean separation of concerns between the user interface (UI) controls and their logic. |
| MVVMLight    | [The MVVM Light Toolkit](http://www.mvvmlight.net/) is a popular, 3rd party framework by Laurent Bugnion, which is based on the [Model-View-ViewModel (MVVM) pattern](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel). The MVVM Light Toolkit helps you separate your View from your Model, which creates applications that are cleaner and easier to extend and maintain. This toolkit puts a special emphasis on the "blend ability" of the created application (the ability to open and edit the user interface into Blend), including the creation of design-time data to enable Blend users to "see something" when they work with data controls. |
| Caliburn.Micro  | [Caliburn.Micro](https://caliburnmicro.com/) is a small, yet powerful framework, designed for building applications across all XAML platforms. Its strong support for MV* patterns will enable you to build your solution quickly, without the need to sacrifice code quality or testability.<br />WTS only supports the use of Caliburn.Micro with projects created in C#. |
| Prism  | [Prism](https://github.com/PrismLibrary/Prism) is a framework for building loosely coupled, maintainable, and testable XAML applications.<br />WTS only supports the use of Prism with projects created in C#. |

### Pages

| Page        | Description |
|------------:|:------------|
| Blank       | This is the most basic page.  A blank canvas to mold into whatever you wish.  The blank page leaves pretty much everything up to you. |
| Settings | The settings page is the page where we recommend putting the configuration settings for your application such as setting a dark / light theme. This could also include any licenses, version number and your privacy terms.|
| Web View | The web view page embeds a view into your app that renders web content using the Microsoft Edge rendering engine. |
| Media Player | A page for displaying video. It includes the MediaPlayer and has the default Media Transport controls enabled.|
| Master/Detail | The master-detail page has a master pane and a details pane for content. When an item in the master list is selected, the details pane is updated. This pattern is frequently used for email and address books. |
| Grid | A page displaying a [RadDataGrid control](http://www.telerik.com/universal-windows-platform-ui/grid), powered by [Telerik UI for UWP](http://www.telerik.com/universal-windows-platform-ui) which is available both [commercially](http://www.telerik.com/purchase/universal-windows-platform) and [open source](https://github.com/telerik/UI-For-UWP). A native, rich and powerful rid with unmatched performance. The grid offers advanced UI virtualization, customizable columns, single and multi-column sorting, data editing, selection and filtering.|
| Chart | A page displaying a [RadChart control](http://www.telerik.com/universal-windows-platform-ui/chart), powered by [Telerik UI for UWP](http://www.telerik.com/universal-windows-platform-ui) which is available both [commercially](http://www.telerik.com/purchase/universal-windows-platform) and [open source](https://github.com/telerik/UI-For-UWP). RadChart control for Windows 10 apps features a rich set of chart series from Bar, Line, Area, Pie, Scatter and Polar charts to different financial series.|
| Tabbed | The tabbed page is used for navigating frequently accessed, distinct content categories. |
| Map | The map page is based around the Windows Map Control. Code includes adding a Map Icon and getting your location. |
| Camera | A page for capturing a photo from the camera. Includes handling previewing, mirroring, and orientation.|
| Image Gallery | A page displaying a image gallery and allows user to navigate between gallery and image detail.|

### Features

| Application Lifecycle | Feature Description |
|-------------------:|:------------|
| Settings Storage | [Setting storage](https://docs.microsoft.com/en-us/uwp/api/windows.storage.applicationdata) is a class that simplifies storage of your application's data, handling loading, saving, serialization, and simplifying access. |
| Suspend and Resume | Enables your app to better handle when a user suspends your app. We do this by hooking into the suspend and resume service, so your app can resume right where it left off. |

| Background Work    | Feature Description |
|-------------------:|:------------|
| Background Task | Creates an [in-process background task](https://docs.microsoft.com/en-us/windows/uwp/launch-resume/create-and-register-an-inproc-background-task), allowing your app to run code when it is not in the foreground. The in-process model enhances the lifecycle of your app with improved notifications, whether your app is in the foreground or background. |

| User Interactions  | Feature Description |
|-------------------:|:------------|
| Toast Notification | Adaptive and interactive toast notifications let you create flexible pop-up notifications that provide users with content, optional inline images, and optional user interactions. You can use pictures, buttons, text inputs, actions, and more. |
| Azure Notification Hubs | [Azure Notification Hubs](https://docs.microsoft.com/en-us/azure/notification-hubs/notification-hubs-push-notification-overview) provide an easy-to-use, multi-platform way to push targeted notifications at Scale. |
| Dev Center Notification | Register your app to receive notifications from the store and handle them being used to launch the app. |
| Live Tile | Enables modification and updates to your app's presence on the Windows 10 Start Menu, providing the ability to change the app's visual state and provide additional context or information. |
| First Run Prompt | Display a prompt when the app is used for the first time. |
| What's New Prompt | Display a prompt when the app is first used after an update. |
| Uri Scheme | Add the ability to launch and deep link into the app with a custom URI scheme.|

## Table of Contents
* [Installing / Using the extension](getting-started-extension.md)
* [**Using and extending your file->new**](getting-started-endusers.md)
* [Concepts of Windows Template Studio](readme.md)
* [Getting started with the generator codebase](getting-started-developers.md)
* [Authoring Templates](templates.md)
