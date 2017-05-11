# Using and extending your generated project

Projects created with Windows Template Studio are intended as a starting point, and will require modification and extension before they're finished. This page explains common ways to extend what is generated for you.

Windows Template Studio approaches UWP app creation using the following four attribute sets to decide how to best generate your app.

## Project Types
**Project types** define the basic look and feel of your UWP app.

| Project type | Description |
|-------------:|:------------|
| Basic | This basic project is a blank canvas upon which to build your app. It provides no scaffolding and leaves everything up to you. |
| [Navigation Pane](projectTypes/navigationpane.md) | This project includes a navigation pane (or 'hamburger menu') at the side of the screen to enable easy user navigation between pages. This style is popular in mobile apps, but also works well on larger screens. The menu can be hidden when space is limited, or when it isn't needed.|
| Pivot and Tabs | Tabs across the top allow for quickly navigating between pages. The pivot control is useful for navigating between related or frequently accessed pages. The user can navigate between pivot panes (pages) by selecting from the text headers, which are always displayed.|


## Frameworks
**App frameworks** define the coding pattern that will be used across the project, tying your UI and code together. Windows Template Studio currently supports the three most common patterns:

| Framework    | Description |
|-------------:|:------------|
| Code Behind  | Code is coupled directly with a XAML file using a .xaml.cs extension. If you developed in WinForms and feel comfortable with that style of development, this is a great option for you. |
| MVVM Basic   | A generic implementation of the [Model-View-ViewModel (MVVM) pattern](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel), which can be used on all XAML platforms. Its intent is to provide a clean separation of concerns between the user interface (UI) controls and their logic. |
| MVVM Light   | [The MVVM Light Toolkit](http://www.mvvmlight.net/) is a popular, 3rd party framework by Laurent Bugnion, which is based on the [Model-View-ViewModel (MVVM) pattern](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel). The MVVM Light Toolkit helps you separate your View from your Model, which creates applications that are cleaner and easier to extend and maintain. This toolkit puts a special emphasis on the "blend ability" of the created application (the ability to open and edit the user interface into Blend), including the creation of design-time data to enable Blend users to "see something" when they work with data controls. |

## Pages

| Page         | Description |
|-------------:|:------------|
| Blank | |
| Map | |
| Master/Detail | |
| [Settings](pages/settings.md) | |
| Tab | |
| Web View | |


## Features


| Application Lifecycle | Feature Description |
|-------------------:|:------------|
| Settings Storage       | [Setting storage](https://docs.microsoft.com/en-us/uwp/api/windows.storage.applicationdata) is a class that simplifies storage of your application's data, handling loading, saving, serialization, and simplifying access. |
| Suspend and Resume     | Enables your app to better handle when a user suspends your app. We do this by hooking into the suspend and resume service, so your app can resume right where it left off. |

| Background Work    | Feature Description |
|-------------------:|:------------|
| Background Task        | Creates an [in-process background task](https://docs.microsoft.com/en-us/windows/uwp/launch-resume/create-and-register-an-inproc-background-task), allowing your app to run code when it is not in the foreground. The in-process moedl enhances the lifecycle of your app with inproved notifications, whether your app is in the foreground or background. |

| User Interactions  | Feature Description |
|-------------------:|:------------|
| Azure Notification Hubs | [Azure Notification Hubs](https://docs.microsoft.com/en-us/azure/notification-hubs/notification-hubs-push-notification-overview) provide an easy-to-use, multi-platform way to push targeted notifications at Scale. |
| Live tiles              | Enables modification and updates to your app's presence on the Windows 10 Start Menu, providing the ability to change the app's visual state and provide additional context or information. |
| Toast Notification      | Adaptive and interactive toast notifications let you create flexible pop-up notifications that provide users with content, optional inline images, and optional user interactions. You can use pictures, buttons, text inputs, actions, and more. |


## Table of Contents

* [Installing / Using the extension](getting-started-extension.md)
* [**Using and extending your file->new**](getting-started-endusers.md)
* [Concepts of Windows Template Studio](readme.md)
* [Getting started with the generator codebase](getting-started-developers.md)
* [Authoring Templates](templates.md)
