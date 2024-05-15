# Using and extending your generated project

WPF projects created with *Template Studio* (aka *TS*) are intended as a starting point, and will require modification and extension before they're finished. This page explains common ways to extend what is generated for you.

## Understanding generated code from Template Studio

The final generated code is the result of a project configuration (project type and design pattern) and a multiple template choice (pages and features).

## Understanding concepts for Template Studio

*Template Studio* approaches WPF app creation using the following attribute sets to decide how to best generate your app.  Below are descriptions of everything you can do.

### Project Types

**Project types** define the basic look and feel of your WPF app.

| Project type | Description |
|-------------:|:------------|
| [Blank](./projectTypes/blank.md) | This basic project is a blank canvas upon which to build your app. It provides no scaffolding and leaves everything up to you. |
| [Navigation Pane](./projectTypes/navigationpane.md) | This project includes a Hamburger Menu at the side of the screen, for navigation between pages. This style is popular in mobile apps but also works well on larger screens. The menu can be expanded or collapsed by clicking on the hamburger button. The menu shows items with an icon and text. The menu can show the icon, or show the icon and text.|
| [Menu Bar](./projectTypes/menubar.md) | The project includes a menu bar on top of the screen that gives access to the pages of the application. Menu Bars are used a lot in desktop applications like Outlook, Word or Visual Studio. The menu initially shows two entries, File and Views. Pages are added to the Views menu entry, Settings Page to the file entry. Once the project is created you can redistribute or create new menu entries as convenient. |
| [Ribbon](./projectTypes/ribbon.md) | Adds a ribbon control which consists of several tabs, grouped by functionality to represent the menu actions of an application. |

### Design patterns

**App Design patterns** define the coding pattern that will be used across the project, tying your UI and code together. *Template Studio* currently supports the three most common patterns:

| Design pattern| Description |
|--------------:|:------------|
| [MVVM Toolkit](./frameworks/mvvmtoolkit.md) | The [CommunityToolkit.Mvvm package](https://aka.ms/mvvmtoolkit) is a modern, fast, and modular MVVM library. It is part of the Windows Community Toolkit. |
| [Code Behind](./frameworks/codebehind.md) | Code is coupled directly with a XAML file using a .xaml.cs extension. If you developed in WinForms and feel comfortable with that style of development, this is a great option for you. |
| [Prism](./frameworks/prism.md) | [Prism](https://github.com/PrismLibrary/Prism) is a framework for building loosely coupled, maintainable, and testable XAML applications. It was originally based on guidance from Microsoft's Patterns and Practices team but is now supported by an open source community. It is designed to help build rich client applications that are flexible and easy to maintain by composing different modules and following design patterns.

### Pages

| Page        | Description |
|------------:|:------------|
| [Blank](./pages/blank.md) | This is the most basic page. A blank canvas to mold into whatever you wish. |
| [Settings](./pages/settings.md)  | The settings page is the page where we recommend putting the configuration settings for your app. |
| [Web View](./pages/webview.md)  | The web view page renders web content using the Microsoft Edge rendering engine. |
| [List/Details](./pages/listdetails.md)  | The list-details page has a list pane and a details pane for content. |
| [Data Grid](./pages/datagrid.md)  | A page displaying a simple data grid. |
| [Content Grid](./pages/contentgrid.md)  | This page contains a basic content grid based on a ListView. |

### Features

#### Application Lifecycle

| Feature | Description |
|-------------:|:-------------|
| [Persist And Restore](./features/persist-and-restore.md) | Persist And Restore helps simplify storing data inside User Local AppData folder. |
| [Multiple views](./features/multiple-views.md) | Enable your users to view independent parts of your app in separate windows. |

#### Packaging

| Feature | Description |
|-------------:|:-------------|
| [MSIX Packaging](./features/msix-packaging.md) | Allows packages creation for side-loading or distribution via Microsoft Store. |

#### User Interactions

| Feature | Description |
|-------------:|:-------------|
| [Theme Selection](./features/theme-selection.md) | Adds theming support to your application. |

### Services

#### Identity (Choose one)

| Service | Description |
|--------:|:------------|
| [Forced Login](./services/forced-login.md) | Make your application require the user to login. |
| [Optional Login](./services/optional-login.md) | Make your application have an optional login and restricted features. |

### Testing

| Testing project | Description |
|-----:|:------------|
| [Test App with MSTest](./testing/app-mstest.md) | Add a project for unit tests against the app using [MSTest](https://github.com/Microsoft/testfx). |
| [Test App with nUnit](./testing/app-nunit.md) | Add a project for unit tests against the app using [NUnit](https://github.com/nunit/docs/wiki/NUnit-Documentation). |
| [Test App with xUnit](./testing/app-xunit.md) | Add a project for unit tests against the app using [xUnit](https://xunit.net/). |
| [Test Core library with MSTest](./testing/core-mstest.md) | Add a project to test code in the Core library with [MSTest](https://github.com/Microsoft/testfx). |
| [Test Core library with nUnit](./testing/core-nunit.md) | Add a project to test code in the Core library with [nUnit](https://nunit.org/). |
| [Test Core library with xUnit](./testing/core-xunit.md) | Add a project to test code in the Core library with [xUnit](https://xunit.net/). |
| [Win App Driver](./testing/win-app-driver.md) | Add project for UI tests using Appium via [Windows Application Driver](https://github.com/Microsoft/WinAppDriver). |

## Learn more

- [MahApps.Metro](./mahapps-metro.md)
- [Adapt the app for specific platforms](./platform-specific-recommendations.md)
- [All docs](../readme.md)
