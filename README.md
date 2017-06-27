# Windows Template Studio

Windows Template Studio is a Visual Studio 2017 Extension that accelerates the creation of new Universal Windows Platform (UWP) apps using a wizard-based experience. The resulting UWP project is well-formed, readable code that incorporates the latest Windows 10 features while implementing proven patterns and best practices. Sprinkled throughout the generated code we have links Docs, Stack Overflow and blogs to provide useful insights.

**Example scenario:**
I need an app that uses MVVM Light, uses master detail, can suspend and resume, settings, maps on one of the pages and will need Azure hub notifications.   It will need a background service that does a query every 5 minutes.

![Windows Template Studio screenshot](docs/resources/getting-started/WPT%20-%20Project%20Type%20and%20Framework.png)



## Build Status

|Env|CI           |Templates Publishing |Extension Publishing |Full Templates Tests|
|:-----|:--------:|:-------------------:|:-------------------:|:------------------:|
|dev|![CI Build](https://winappstudio.visualstudio.com/_apis/public/build/definitions/5c80cfe7-3bfb-4799-9d04-803c84df7a60/121/badge) | ![Templates Publishing Build](https://winappstudio.visualstudio.com/_apis/public/build/definitions/5c80cfe7-3bfb-4799-9d04-803c84df7a60/123/badge) | ![Extension Publishing Build](https://winappstudio.visualstudio.com/_apis/public/build/definitions/5c80cfe7-3bfb-4799-9d04-803c84df7a60/122/badge)|![Full Templates Tests](https://winappstudio.visualstudio.com/_apis/public/build/definitions/5c80cfe7-3bfb-4799-9d04-803c84df7a60/128/badge)|
|pre-release|![CI Build](https://winappstudio.visualstudio.com/_apis/public/build/definitions/5c80cfe7-3bfb-4799-9d04-803c84df7a60/125/badge)|![Templates Publishing Build](https://winappstudio.visualstudio.com/_apis/public/build/definitions/5c80cfe7-3bfb-4799-9d04-803c84df7a60/124/badge)|![Extension Publishing Build](https://winappstudio.visualstudio.com/_apis/public/build/definitions/5c80cfe7-3bfb-4799-9d04-803c84df7a60/126/badge)|![Full Templates Tests](https://winappstudio.visualstudio.com/_apis/public/build/definitions/5c80cfe7-3bfb-4799-9d04-803c84df7a60/129/badge)|



## Features
Windows Template Studio approaches UWP app creation using the following four attribute sets:
* **Project type**: First, how do you want your app's UI navigation to behave? We currently support three project types: *basic*, *[navigation pane](docs/projectTypes/navigationpane.md)*, and *pivot and tabs*
* **App framework**: Next, what coding pattern do you want to use in your project, we currently support three common patterns: *code behind*, *basic MVVM*, and *[MVVM Light](http://www.mvvmlight.net/)*
* **App pages**: To accelerate app creation, we provide a number of app page templates that you can use to add common UI pages into your new app. We currently include page templates from the *blank page* to the common layouts (*e.g., master/detail, tabbed, web view*) to pages that implement common patterns (*e.g., [app settings](docs/pages/settings.md), map control*). Using the wizard, add as many of the pages as you need, providing a name for each one, and we'll generate them for you.
* **Windows 10 features**: Lastly, you specify which UWP capabilities you want to use in your app, and we'll build out the framework for the features into your app, tagging 'TODO' items. Currently supported features cover application lifecycle (*settings storage, suspend and resume*), background tasks, and user interaction (*app notifications, Live tiles, and Azure Notification Hub*).

Once you select the attributes you want your new UWP app to have, you can quickly [extend the generated code](docs/getting-started-endusers.md).

## Known Issues

The following are known issues and may affect your use of Windows Template Studio. These will be addressed in the next few days.

* You must be online the first time you use the wizard to generate an app or no templates will be loaded. [#311](https://github.com/Microsoft/WindowsTemplateStudio/issues/311)
* Only compatible with Windows 10. You cannot use this if running VS 2017 on an older Operating System. [#357](https://github.com/Microsoft/WindowsTemplateStudio/issues/357)

## Documentation

* [Installing / Using the extension](docs/getting-started-extension.md)
* [Using and extending your generated project](docs/getting-started-endusers.md)
* [Concepts of Windows Template Studio](docs/readme.md)
* [Getting started with the generator codebase](docs/getting-started-developers.md)
* [Authoring Templates](docs/templates.md)


## Feedback, Requests and Roadmap

Please use [GitHub issues](https://github.com/Microsoft/WindowsTemplateStudio/issues) for feedback, questions or comments.  

If you have specific feature requests or would like to vote on what others are recommending, please go to the [GitHub issues](https://github.com/Microsoft/WindowsTemplateStudio/issues) section as well.  We would love to see what you are thinking.

Here is what we're currently thinking in our [roadmap](docs/roadmap.md)

## Contributing

Do you want to contribute? Here are our [contribution guidelines](CONTRIBUTING.md).

## Principles

1. Generated templates will be kept simple.
1. Generated templates are a starting point, not a completed application.
1. Generated templates once generated, must be able to be compiled and run.
1. Generated templates should work on all device families.
1. Templates should have comments to aid developers.  This includes links to signup pages for keys, MSDN, blogs and how-to's.  All guidance provide should be validated from either the framework/SDK/library’s creator.
1. All features will be supported for two current Windows SDK for Windows 10 release cycles or until another principle supersedes it.
1. Templates released in production will try to adhere to the design language used in the current release of Windows 10.
1. Code should follow [.NET Core coding style](https://github.com/dotnet/corefx/blob/master/Documentation/coding-guidelines/coding-style.md)

## License

This code is distributed under the terms and conditions of the [MIT license](LICENSE.md).

## Code of Conduct

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Privacy Statement

The extension does [log basic telemetry](docs/telemetry.md) for what is being selected. Please read the [Microsoft privacy statement](http://go.microsoft.com/fwlink/?LinkId=521839) for more information.
