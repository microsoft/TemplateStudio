# Using and extending your generated project

UWP projects created with Windows Template Studio (WinTS) are intended as a starting point, and will require modification and extension before they're finished. This page explains common ways to extend what is generated for you.

## Understanding generated code from Windows Template Studio

The final generated code is the result of a project configuration (project type and design pattern) and a multiple template choice (pages and features). There are a few concepts to understand before start working on the generated code.

- [Application activation](./activation.md)
- [Navigation between pages](./navigation.md)
- [Notifications in Windows Template Studio](./notifications.md)

## Understanding concepts for Windows Template Studio

Windows Template Studio approaches UWP app creation using the following attribute sets to decide how to best generate your app.  Below are descriptions of everything you can do.

### Project Types

**Project types** define the basic look and feel of your UWP app.

| Project type | Description |
|-------------:|:------------|
| [Blank](./projectTypes/blank.md) | This basic project is a blank canvas upon which to build your app. It provides no scaffolding and leaves everything up to you. |
| [Navigation Pane](./projectTypes/navigationpane.md) | This project includes a navigation pane (or 'hamburger menu') at the side of the screen to enable easy user navigation between pages. This style is popular in mobile apps, but also works well on larger screens. The menu can be hidden when space is limited, or when it isn't needed.|
| [Horizontal Navigation Pane](./projectTypes/horizontalnavigationpane.md) | Navigation Items across the top allow for quickly navigating between pages. This is recommended if you have 5 or fewer top level navigation items. The user can navigate between pages by selecting from the text headers, which are always displayed.|
| [Menu Bar](./projectTypes/menubar.md) | This project contains a top menu bar with File and Views options and a Blank canvas to show your views. The project includes navigation methods to show views in different ways. |

### Design patterns

**App Design patterns** define the coding pattern that will be used across the project, tying your UI and code together. Windows Template Studio currently supports the following common patterns:

| Design pattern| Description |
|--------------:|:------------|
| [MVVM Toolkit](./frameworks/mvvmtoolkit.md) | The [Microsoft.Toolkit.Mvvm package](https://aka.ms/mvvmtoolkit) is a modern, fast, and modular MVVM library. It is part of the Windows Community Toolkit. |
| [Code Behind](./frameworks/codebehind.md) | Code is coupled directly with a XAML file using a .xaml.cs extension. If you developed in WinForms and feel comfortable with that style of development, this is a great option for you. |
| [Prism](./frameworks/prism.md) | [Prism](https://github.com/PrismLibrary/Prism) is a framework for building loosely coupled, maintainable, and testable XAML applications. It was originally based on guidance from Microsoft's Patterns and Practices team but is now supported by an open source community. It is designed to help build rich client applications that are flexible and easy to maintain by composing different modules and following design patterns.<br />WinTS only supports the use of Prism with projects created in C#. |
| [MVVM Basic](./frameworks/mvvmbasic.md) | MVVM Basic includes the minimum necessary to follow the MVVM pattern without including any extra libraries or dependencies. It is intended for people new to MVVM or who are unable to or do not wish to use a 3rd party framework. |
| [MVVMLight](./frameworks/mvvmlight.md) | The [MVVM Light Toolkit](http://www.mvvmlight.net/) is a popular, 3rd party toolkit by Laurent Bugnion, which has the purpose of accelerating the creation and development of MVVM applications.  This toolkit puts a special emphasis on the "blend ability" of the created application (the ability to open and edit the user interface into Blend), including the creation of design-time data to enable Blend users to "see something" when they work with data controls.<br />As a toolkit, it provides a number of tools and features but there is no requirement to use all of them. This toolkit is popular with developers who want use parts of it to take care of the basics but still allow them to structure the code in their own way. |
| [Caliburn.Micro](./frameworks/caliburnmicro.md) | [Caliburn.Micro](https://caliburnmicro.com/) is a small, yet powerful framework, designed for building applications across all XAML platforms. Its strong support for MV* patterns will enable you to build your solution quickly, without the need to sacrifice code quality or testability.<br />It uses a convention based approach to mapping actions/events, bindings, and views to view models. While this framework is highly opinionated about how it should be used and apps should be constructed, it does allow for customization of it's behavior.<br />WinTS only supports the use of Caliburn.Micro with projects created in C#. |

### Pages

| Page        | Description |
|------------:|:------------|
| [Blank](./pages/blank.md) | This is the most basic page. A blank canvas to mold into whatever you wish. |
| [Settings](./pages/settings.md)  | The settings page is the page where we recommend putting the configuration settings for your app. |
| [Web View](./pages/webview.md)  | The web view page renders web content using the Microsoft Edge rendering engine. |
| [Media Player](./pages/mediaplayer.md)  | A page for showing video with system media controls enabled. |
| [Master/Detail](./pages/masterdetail.md)  | The master-detail page has a master pane and a details pane for content. |
| [TreeView](./pages/treeview.md)  | The tree-view page has a tree view control to show hierarchical content. |
| [Content Grid](./pages/contentgrid.md)  | This page allows you to add custom items in the form of an Adaptive Grid. |
| [DataGrid](./pages/datagrid.md)  | A page displaying a simple data grid. |
| [Telerik Data Grid](./pages/telerikdatagrid.md)  | A page displaying a simple grid, powered by Telerik UI controls for UWP. |
| [Chart](./pages/chart.md)  | A page displaying a simple chart, powered by Telerik UI controls for UWP. |
| [TabbedPivot](./pages/tabbed.md)  | The tabbed page is used for navigating frequently accessed, distinct content categories. |
| [TabView](./pages/tabview.md)  | The tab view page has a TabView control to show contents in different tabs. |
| [TwoPaneView](./pages/twopaneview.md)  | Master-Detail page optimized for dual-screen devices. |
| [Map](./pages/map.md)  | The map page is based around the Windows Map Control. Code includes adding a Map Icon and getting your location. |
| [Camera](./pages/camera.md)  | A page for capturing and displaying an image from the camera. |
| [Image Gallery](./pages/imagegallery.md)  | A page displaying a image gallery and allows user to navigate between gallery and image detail. |
| [Ink Draw](./pages/inkdraw.md)  | A page that allows you to create notes and drawings using Windows Ink. |
| [Ink Smart Canvas](./pages/inksmartcanvas.md)  | A page that allows you to create notes using shape and text recognition with Windows Ink. |
| [Ink Draw Picture](./pages/inkdrawpicture.md)  | A page that allows you to load a picture and draw on top of it using Windows Ink. |

### Features

#### Analytics

| Feature | Description |
|-------------:|:-------------|
| [VS App Center Analytics](./features/vs-app-center-analytics.md) | Send analytics to the Visual Studio App Center. |

#### Application Launching

| Feature | Description |
|-------------:|:-------------|
| [3D App Launcher](./features/3d-app-launcher.md) | Include a 3D app launcher when the app is used in a Mixed Reality environment. |
| [Deep Linking](./features/deep-linking.md) | Add the ability to launch and deep link into the app with a custom URI scheme. |
| [UserActivity](./features/user-activity.md) | Add the ability to add user activities to the Windows Timeline. |
| [Web to App link](./features/web-to-app-link.md) | Web to App link associates your app with a website so that when someone opens a link to your website. |
| [Command Line Launch](./features/command-line.md) | Support passing arguments and launching from the command line. |
| [Multi-Instance](./features/multi-instance.md) | Launch multiple instances of the app. |
| [Multi-Instance Advanced](./features/multi-instance-advanced.md) | Control how multiple instances of the app are launched. |

#### Application Lifecycle

| Feature | Description |
|-------------:|:-------------|
| [Settings Storage](./features/setting-storage.md) | Setting storage helps simplify storing data inside your application data folder. |
| [Suspend and Resume](./features/suspend-and-resume.md) | A simple service you can hook into to handle when a user leaves and returns to the app. |
| [Multiple Views](./features/multiple-views.md) | Enable your users to view independent parts of your app in separate windows. |

#### Background Work

| Feature | Description |
|-------------:|:-------------|
| [Background Task](./features/background-task.md) | Add an in-process background task ready to run code even while the app is not in the foreground. |

#### Connected Experiences

| Feature | Description |
|-------------:|:-------------|
| [Share Source](./features/share-source.md) | Helps supporting the share contract in your UWP app to share data from your app with others. |
| [Share Target](./features/share-target.md) | Allows you to receive data shared by other apps using the share contract in your UWP app. |

#### User Interactions

| Feature | Description |
|-------------:|:-------------|
| [Toast Notification](./features/toast-notifications.md) | Adds code to show displaying a toast and handling it being used to launch the app. |
| [Azure Notifications](./features/azure-notifications.md) | Register to receive and handle messages from Azure Notification Hubs. |
| [Dev Center Notification](./features/dev-center-notifications.md) | Register your app to receive notifications from the store. |
| [Live Tile](./features/live-tile.md) | Adds a sample to show updating the tile as well as code for working with multiple tiles. |
| [First Run Prompt](./features/first-run-prompt.md) | Display a prompt when the app is used for the first time. |
| [What's New Prompt](./features/whats-new-prompt.md) | Display a prompt when the app is first used after an update. |
| [Feedback Hub Link](./features/feedback-hub-link.md) | Add a link, on the settings page, to the Feedback Hub. |
| [Drag & Drop](./features/drag-and-drop.md) | The Drag & Drop feature provides a service to simplify the creation of drag and drop ready apps. |
| [Theme Selection](./features/theme-selection.md) | Adds theming support to your application. |

### Services

#### Data

| Service | Description |
|--------:|:------------|
| [HTTP Data Service](./services/http-data-service.md) | Access content over HTTP. |
| [Web API](./services/web-api.md) | Include ASP.NET Core Web API project. |
| [Secured Web API](./services/secured-web-api.md) | Include a ASP.NET Core Web API project that validates a JWToken. |
| [SQL Server Data](./services/sql-server-data-service.md) | Get data from SQL Server to use in the app. |

#### Authentication (Select one)

| Service | Description |
|--------:|:------------|
| [Forced Login](./services/forced-login.md) | Make your application require the user to login. |
| [Optional Login](./services/optional-login.md) | Make your application have an optional login and restricted features. |

#### Tools

| Service | Description |
|--------:|:------------|
| [XAML Styler Config](./services/xaml-styler-config.md) | Default [XAML Styler](https://marketplace.visualstudio.com/items?itemName=NicoVermeir.XAMLStyler) config file. |

### Testing

| Name | Description |
|-----:|:------------|
| [Test App with MSTest](./testing/app-mstest.md) | Add a project for unit tests against the app using [MSTest](https://github.com/Microsoft/testfx). |
| [Test App with xUnit](./testing/app-xunit.md) | Add a project for unit tests against the app using [xUnit](https://xunit.net/). |
| [Test Core library with MSTest](./testing/core-mstest.md) | Add a project to test code in the Core library with [MSTest](https://github.com/Microsoft/testfx). |
| [Test Core library with nUnit](./testing/core-nunit.md) | Add a project to test code in the Core library with [nUnit](https://nunit.org/). |
| [Test Core library with xUnit](./testing/core-xunit.md) | Add a project to test code in the Core library with [xUnit](https://xunit.net/). |
| [Win App Driver](./testing/win-app-driver.md) | Add project for UI tests using Appium via [Windows Application Driver](https://github.com/Microsoft/WinAppDriver). |

---

## Pre-requisies

The minimum supported Windows 10 version and which SDKs should be pre-installed are detailed in the [WinTS Principles](../../README.md#principles).

## Learn more

- [Handling app activation](./activation.md)
- [Handling navigation within the app](./navigation.md)
- [Adapt the app for specific platforms](./platform-specific-recommendations.md)
- [All docs](../readme.md)
