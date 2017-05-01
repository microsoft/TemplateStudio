# Windows Template Studio

Windows Template Studio goal is to help developers with their File->New experience in Visual Studio. It will generate a strong, generic foundation with the pages you need, but also integrate game changing features from the start. Once the template is generated for the developer, it can provide base sample data and will be able to compile then run without issue. Sprinkled throughout the generated code we have links Docs, Stack Overflow and blogs to provide useful insights.

**Example scenario:**
I need an app that uses MVVM Light, uses master detail, can suspend and resume, settings, maps on one of the pages and will need Azure hub notifications.   It will need a background service that does a query every 5 minutes.

## Table of Contents

* [Installing / Using the extension](docs/getting-started-extension.md)
* [Using and extending your file->new](docs/getting-started-endusers.md)
* [Concepts of Windows Template Studio](docs/readme.md)
* [Getting started with the generator codebase](docs/getting-started-developers.md)
* [Authoring Templates](docs/templates.md)

## Build Status

|Env|CI           |Templates Publishing |Extension Publishing |Full Templates Tests|
|:-----|:--------:|:-------------------:|:-------------------:|:------------------:|
|dev|![CI Build](https://winappstudio.visualstudio.com/_apis/public/build/definitions/5c80cfe7-3bfb-4799-9d04-803c84df7a60/121/badge) | ![Templates Publishing Build](https://winappstudio.visualstudio.com/_apis/public/build/definitions/5c80cfe7-3bfb-4799-9d04-803c84df7a60/123/badge) | ![Extension Publishing Build](https://winappstudio.visualstudio.com/_apis/public/build/definitions/5c80cfe7-3bfb-4799-9d04-803c84df7a60/122/badge)|![Full Templates Tests](https://winappstudio.visualstudio.com/_apis/public/build/definitions/5c80cfe7-3bfb-4799-9d04-803c84df7a60/128/badge)|
|pre-release|![CI Build](https://winappstudio.visualstudio.com/_apis/public/build/definitions/5c80cfe7-3bfb-4799-9d04-803c84df7a60/125/badge)|![Templates Publishing Build](https://winappstudio.visualstudio.com/_apis/public/build/definitions/5c80cfe7-3bfb-4799-9d04-803c84df7a60/124/badge)|![Extension Publishing Build](https://winappstudio.visualstudio.com/_apis/public/build/definitions/5c80cfe7-3bfb-4799-9d04-803c84df7a60/126/badge)|![Full Templates Tests](https://winappstudio.visualstudio.com/_apis/public/build/definitions/5c80cfe7-3bfb-4799-9d04-803c84df7a60/129/badge)|

## Features

### Supported Project Types

* Basic
* Navigation Pane
* Pivot and tabs

### Supported Frameworks

* MVVM Basic
* [MVVM Light](http://www.mvvmlight.net/)
* Code behind

### Supported Pages

* Blank page
* Map page
* Master/Detail page
* [Settings page](docs/pages/settings.md)
* Tabbed page
* Web view page

### Supported Features

* Background tasks
* Setting storage
* Suspend and resume
* Azure-based Notifications (Azure Notification Hub)
* Live Tiles
* Toast Notifications

## Feedback, Requests and Roadmp

Please use [GitHub issues](https://github.com/Microsoft/WindowsTemplateStudio/issues) for feedback, questions or comments.  

If you have specific feature requests or would like to vote on what others are recommending, please go to the [GitHub issues](https://github.com/Microsoft/WindowsTemplateStudio/issues) section as well.  We would love to see what you are thinking.

Here is what we're currently thinking in our [roadmap](docs/roadmap.md)

## Contributing

Do you want to contribute? Here are our [contribution guidelines](contributing.md).

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

This code is distributed under the terms and conditions of the [MIT license](license.md).

## Code of Conduct

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Privacy Statement

The extension does [log basic telemetry](docs/telemetry.md) for what is being selected. Please read the [Microsoft privacy statement](http://go.microsoft.com/fwlink/?LinkId=521839) for more information.
