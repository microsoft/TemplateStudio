|Env|CI        |Templates Publishing |Extension Publishing |
|:-----|:--------:|:-------------------:|:-------------------:|
|dev|![CI Build](https://winappstudio.visualstudio.com/_apis/public/build/definitions/5c80cfe7-3bfb-4799-9d04-803c84df7a60/121/badge) | ![Templates Publishing Build](https://winappstudio.visualstudio.com/_apis/public/build/definitions/5c80cfe7-3bfb-4799-9d04-803c84df7a60/123/badge) | ![Extension Publishing Build](https://winappstudio.visualstudio.com/_apis/public/build/definitions/5c80cfe7-3bfb-4799-9d04-803c84df7a60/122/badge)|
|pre-release|![CI Build](https://winappstudio.visualstudio.com/_apis/public/build/definitions/5c80cfe7-3bfb-4799-9d04-803c84df7a60/125/badge)|![Templates Publishing Build](https://winappstudio.visualstudio.com/_apis/public/build/definitions/5c80cfe7-3bfb-4799-9d04-803c84df7a60/124/badge)|![Extension Publishing Build](https://winappstudio.visualstudio.com/_apis/public/build/definitions/5c80cfe7-3bfb-4799-9d04-803c84df7a60/126/badge)|

Windows Template Studio 
===========
Windows Template Studio goal is to help developers with their File->New experience in Visual Studio.  It will generate a strong, generic foundation with the pages you need, but also integrate game changing features like Cortana, action center and background tasks from the start.  Any critical features will have code comments with links to MSDN, stack overflow and blogs to help unblock developers. Once the template is generated for the developer, it can provide base sample data and will be able to compile then run without issue.

Example scenario:
I need an app that uses MVVM Light, uses Cortana, Speech APIs, Ink on one of the pages and will need Azure mobile services.   It will need a background service that does a query every 5 minutes.

To reach our developers in an up-to-date fashion, the project is broken up into two primary parts, templates and the generator.  The generator is built on top of [dotnet Template Engine](https://github.com/dotnet/templating), it is a Visual Studio extension a developer will install while the templates will be hosted on a CDN so we can update what is created independntly of the generator.  The generator uses templates to create actual projects, pages and/or features for the developers. 

## Getting Started
Please read the [Getting Started with Windows Template Studio](docs/getting-started.md) page for more detailed information about using Windows Template Studio.

To start working with the latest version of this code, check [Getting Started for Developers](docs/getting-started-developers.md)

To start using the Windows Template Studio extension, check [Getting Started with the Extension](docs/getting-started-extension.md)

To start authoring templates, check [Understanding the Templates](docs/templates.md)

## Features

### Supported Application Types
 * Basic

### Supported Frameworks
 * Basic
 * [MVVM Light](http://www.mvvmlight.net/)

### Supported Pages
Coming soon

### Developer Options
Coming soon

### End User Options
Coming soon

## Feedback and Requests
Please use [GitHub issues](https://github.com/Microsoft/WindowsTemplateStudio/issues) for questions or comments.  If you have specific feature requests or would like to vote on what others are recommending, please go to the [GitHub issues](https://github.com/Microsoft/WindowsTemplateStudio/issues)section as well.  We would love to see what you are thinking.

## Contributing
Do you want to contribute? Here are our [contribution guidelines](contributing.md).

## Principles
 * Principle #1: Generated templates will be kept simple.
 * Principle #2: Generated templates are a starting point, not a completed application.
 * Principle #3: Generated templates once generated, must be able to be compiled and run.
 * Principle #4: Generated templates should work on all device families.
 * Principle #5: Formulas should have comments to aid developers.  This includes links to singup pages for keys, MSDN, blogs and how-to's.  All guidance provide should be validated from either the framework/SDK/library’s creator.
 * Principle #6: All features will be supported for two Windows SDK for Windows 10 release cycles or until another principle supersedes it.
 * Principle #7: Templates released in production will try to adhere to the design language used in the current release of Windows 10.

This project has adopted the code of conduct defined by the [http://contributor-covenant.org/](Contributor Covenant) to clarify expected behavior in our community. 

## Roadmap
Read what we [plan for next iteration](https://github.com/Microsoft/WindowsTemplateStudio/issues?q=is%3Aopen+is%3Aissue+milestone%3A0.5), and feel free to ask questions.

You can add [this feed](https://www.myget.org/F/vsixextensions/vsix/) to your Extensions galleries and you can get the pre-release versions of the Windows Template Studio Visual Studio Extension. 

## License
This code is distributed under the terms and conditions of the [MIT license](license.md). 

## Code of Conduct

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

