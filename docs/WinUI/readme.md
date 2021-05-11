# WinUI 3 Apps

WinUI 3 project templates supported by Windows Template Studio:

C# templates:
- App (WinUI 3 Desktop)
- Class Library (WinUI 3 in Desktop)

C++ Templates:
- App (WinUI 3 Desktop)
- Windows Runtime Component (WinUI 3)

We also offer the following item templates for C# and C++:
- Blank page
- Blank Window (desktop)
- Custom Control
- Resource Dictionary
- Resources File
- User Control

# Using and extending your generated project

WinUI 3 projects created with Windows Template Studio (WinTS) are intended as a starting point, and will require modification and extension before they're finished. This page explains common ways to extend what is generated for you.

## Understanding generated code from Windows Template Studio

The final generated code is the result of a project configuration (project type and design pattern) and a multiple template choice (pages and features). There are a few concepts to understand before start working on the generated code.

- [Application activation](./activation.md)
- [Navigation between pages](./navigation.md)

## Understanding concepts for Windows Template Studio

Windows Template Studio approaches WinUI 3 app creation using the following attribute sets to decide how to best generate your app.  Below are descriptions of everything you can do.

### Project Types

**Project types** define the basic look and feel of your wINui 3 app.

| Project type | Description |
|-------------:|:------------|
| [Blank](./projectTypes/blank.md) | This basic project is a blank canvas upon which to build your app. It provides no scaffolding and leaves everything up to you. |
| [Navigation Pane](./projectTypes/navigationpane.md) | This project includes a navigation pane (or 'hamburger menu') at the side of the screen, for navigation between pages. This style is popular in mobile apps but also works well on larger screens. The menu can be hidden when space is limited, or it isn't needed. The menu shows items with an icon and text. The menu can be entirely hidden, show just the icon, or show the icon and text. The user can choose to display the full menu at the touch of a button. The menu also adapts automatically to the size of the screen. |

### Design patterns

**App Design patterns** define the coding pattern that will be used across the project, tying your UI and code together. Windows Template Studio currently supports the following common patterns:

| Design pattern| Description |
|--------------:|:------------|
| [MVVM Toolkit](./frameworks/mvvmtoolkit.md) | The [Microsoft.Toolkit.Mvvm package](https://aka.ms/mvvmtoolkit) is a modern, fast, and modular MVVM library. It is part of the Windows Community Toolkit. |

### Pages

| Page        | Description |
|------------:|:------------|
| [Blank](./pages/blank.md) | This is the most basic page. A blank canvas to mold into whatever you wish. |
| [Content Grid](./pages/content-grid.md) | This page allows you to add custom items in the form of an Adaptive Grid. |
| [Data Grid](./pages/data-grid.md) | A page displaying a simple data grid. |
| [List Details](./pages/list-details.md) | The list/details pattern has a list pane and a details pane for content. |
| [Settings](./pages/settings.md) | The settings page is the page where we recommend putting the configuration settings for your app. |
| [WebView](./pages/web-view.md) | The web view page renders web content using the Microsoft Edge rendering engine. |

### Features

#### Application Lifecycle

| Feature | Description |
|-------------:|:-------------|
| [Settings Storage](./features/setting-storage.md) | Setting storage helps simplify storing data inside your application data folder. |

#### Packaging

| Feature | Description |
|-------------:|:-------------|
| [MSIX](./features/msix.md) | Allows packages creation for side-loading or distribution via Microsoft Store. |

#### User Interactions

| Feature | Description |
|-------------:|:-------------|
| [Theme Selection](./features/theme-selection.md) | Adds theming support to your application. |


### Known issues:
- [Dark/Light theme issue](https://github.com/microsoft/WindowsTemplateStudio/issues/4087)
- [VisualStateManager initialization issue](https://github.com/microsoft/WindowsTemplateStudio/issues/4072)

### Additional docs:
- [Windows UI Library 3 - Project Reunion 0.5 (March 2021)](https://docs.microsoft.com/windows/apps/winui/winui3/)
- [Windows UI Library on GitHub](https://github.com/Microsoft/microsoft-ui-xaml)
- [Project Reunion on GitHub](https://github.com/microsoft/ProjectReunion)
- [Windows Community Toolkit for Project Reunion 0.5](https://devblogs.microsoft.com/ifdef-windows/windows-community-toolkit-for-project-reunion-0-5/)
- [Update existing projects to the latest release of Project Reunion](https://docs.microsoft.com/windows/apps/project-reunion/update-existing-projects-to-the-latest-release)
