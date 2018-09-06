# Windows Template Studio

Windows Template Studio (WTS) is a Visual Studio 2017 Extension that accelerates the creation of new Universal Windows Platform (UWP) apps using a wizard-based experience. The resulting UWP project is well-formed, readable code that incorporates the latest Windows 10 features while implementing proven patterns and best practices. Sprinkled throughout the generated code we have links Docs, Stack Overflow and blogs to provide useful insights. WTS supports creating apps in C# or VB.Net.

**Example scenario:**
I need an app that uses MVVM Light, uses master detail, can suspend and resume, settings, maps on one of the pages and will need Azure hub notifications.   It will need a background service that does a query every 5 minutes.

![Windows Template Studio screenshot](docs/resources/getting-started/WTS%20-%20Project%20Type.png)

## Build Status

|Branch   |CI                |Test Version|Version|
|:--------|:----------------:|:---------------:|:---------------:|
|master|[![Build status](https://ci.appveyor.com/api/projects/status/nf8r35r45o4yqbqs/branch/master?svg=true)](https://ci.appveyor.com/project/ralarcon/windowstemplatestudio/branch/master)|[![Prerelease Version](https://wtsrepository.blob.core.windows.net/badges/img.prerelease.version.svg)](https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/getting-started-extension.md#nightly--pre-release-feeds-for-windows-template-studio) |[![Production Version](https://wtsrepository.blob.core.windows.net/badges/img.release.version.svg?maxAge=600)](https://marketplace.visualstudio.com/items?itemName=WASTeamAccount.WindowsTemplateStudio)|
|dev|[![Build status](https://ci.appveyor.com/api/projects/status/nf8r35r45o4yqbqs/branch/dev?svg=true)](https://ci.appveyor.com/project/ralarcon/windowstemplatestudio/branch/dev)|[![Nightly Version](https://wtsrepository.blob.core.windows.net/badges/img.nightly.version.svg)](https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/getting-started-extension.md#nightly--pre-release-feeds-for-windows-template-studio)||

|Branch   |Full Tests       |WACK Tests       |
|:--------|:---------------:|:---------------:|
|master|[![Full Integration Tests](https://winappstudio.visualstudio.com/_apis/public/build/definitions/5c80cfe7-3bfb-4799-9d04-803c84df7a60/129/badge)](https://github.com/Microsoft/WindowsTemplateStudio/blob/vsts-builds/docs/vsts-builds/129.md)|[![Wack Tests](https://winappstudio.visualstudio.com/DefaultCollection/_apis/public/build/definitions/5c80cfe7-3bfb-4799-9d04-803c84df7a60/144/badge)](https://github.com/Microsoft/WindowsTemplateStudio/blob/vsts-builds/docs/vsts-builds/144.md)
|dev|[![Full Integration Tests](https://winappstudio.visualstudio.com/_apis/public/build/definitions/5c80cfe7-3bfb-4799-9d04-803c84df7a60/128/badge)](https://github.com/Microsoft/WindowsTemplateStudio/blob/vsts-builds/docs/vsts-builds/128.md)|[![Wack Tests](https://winappstudio.visualstudio.com/DefaultCollection/_apis/public/build/definitions/5c80cfe7-3bfb-4799-9d04-803c84df7a60/142/badge)](https://github.com/Microsoft/WindowsTemplateStudio/blob/vsts-builds/docs/vsts-builds/142.md)

> The builds include test verifications to validate the contributions:
> * *CI Build*: Includes all unit test + minimum integration verifications (minimum generation + build + code style rules). Runs every PR requested / PR accepted.
> * *Full Tests*: Includes tests to verify combinations and variations of templates from a project generation point of view and builds the solutions generated to ensure no build time issues found. Runs every PR accepted and takes longer to be completed.
> * *Wack Tests*: Includes tests that run the App Certification Kit against the generated projects to ensure there are no issues blocking a submission to the store. Runs once nightly and takes quite a while to complete.

## Features

Windows Template Studio approaches UWP app creation using the following four attribute sets:

* **Project type**: First, how do you want your app's UI navigation to behave? We currently support three project types: *basic*, *[navigation pane](docs/projectTypes/navigationpane.md)*, and *pivot and tabs*.
* **App design pattern**: Next, what coding pattern do you want to use in your project, we currently support three common patterns: *code behind*, *basic MVVM*, *[MVVMLight](http://www.mvvmlight.net/)*, *[Caliburn.Micro](https://caliburnmicro.com/)*, and *[Prism](https://github.com/PrismLibrary/Prism)*.
* **App pages**: To accelerate app creation, we provide a number of app page templates that you can use to add common UI pages into your new app. We currently include page templates from the *blank page* to the common layouts (*e.g., master/detail, tabbed, web view*) to pages that implement common patterns (*e.g., [app settings](docs/pages/settings.md), map control*). Using the wizard, add as many of the pages as you need, providing a name for each one, and we'll generate them for you.
* **Windows 10 features**: Lastly, you specify which UWP capabilities you want to use in your app, and we'll build out the framework for the features into your app, tagging 'TODO' items. Currently supported features cover application lifecycle (*settings storage, suspend and resume*), background tasks, and user interaction (*app notifications, Live tiles, and Azure Notification Hub*).

Once you select the attributes you want your new UWP app to have, you can quickly [extend the generated code](docs/getting-started-endusers.md).

## Documentation

* [Installing / Using the extension](docs/getting-started-extension.md)
* [Using and extending your generated project](docs/getting-started-endusers.md)
* [Concepts of Windows Template Studio](docs/readme.md)
* [Getting started with the generator codebase](docs/getting-started-developers.md)
* [Authoring Templates](docs/templates.md)

## Known issues
* Issue ([#1532](https://github.com/Microsoft/WindowsTemplateStudio/issues/1532)): when uninstalling / upgrading where you may get an error of "A value for 'Component' needs to be specified in the catalog."  If you get this error, we need logs to help track this with the help of the Visual Studio team.  We don't know how to reproduce it but we know a few people have hit this scenario.  We have how to capture these logs in the [tracking issue on GitHub.](https://github.com/Microsoft/WindowsTemplateStudio/issues/1532)
* You can't have side-by-side versions (nightly/pre-release/release) of WindowsTemplateStudio VSPackage into a single instance of Visual Studio.

## Feedback, Requests and Roadmap

Please use [GitHub issues](https://github.com/Microsoft/WindowsTemplateStudio/issues) for feedback, questions or comments.

If you have specific feature requests or would like to vote on what others are recommending, please go to the [GitHub issues](https://github.com/Microsoft/WindowsTemplateStudio/issues) section as well.  We would love to see what you are thinking.

Here is what we're currently thinking in our [roadmap](docs/roadmap.md).

## Contributing

Do you want to contribute? We would love to have you help out. Here are our [contribution guidelines](CONTRIBUTING.md).

## Principles

1. Generated templates will be kept simple.
1. Generated templates are a starting point, not a completed application.
1. Generated templates once generated, must be able to be compiled and run.
1. Generated templates should work on all device families.
1. Templates should have comments to aid developers.  This includes links to signup pages for keys, MSDN, blogs and how-to's.  All guidance provide should be validated from either the framework/SDK/library’s creator.
1. All features will be supported for two most recent RTM Windows 10 Updates. Those supported releases are Windows 10 Creators Update and Windows 10 Fall Creators Update.
1. Templates released in production will try to adhere to the design language used in the current release of Windows 10.
1. Code should follow [.NET Core coding style](https://github.com/dotnet/corefx/blob/master/Documentation/coding-guidelines/coding-style.md).

This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behavior in our community.
For more information see the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct).

## License

This code is distributed under the terms and conditions of the [MIT license](LICENSE.md).

## Privacy Statement

The extension does [log basic telemetry](docs/telemetry.md) for what is being selected. Please read the [Microsoft privacy statement](http://go.microsoft.com/fwlink/?LinkId=521839) for more information.

## .NET Foundation

This project is supported by the [.NET Foundation](https://dotnetfoundation.org).

## Projects we like and collaborate with

- [Rapid Xaml Toolkit](https://github.com/Microsoft/Rapid-XAML-Toolkit)
- [Windows Community Toolkit](https://github.com/Microsoft/WindowsCommunityToolkit)

## Libraries and frameworks we use in our generated projects

- [Caliburn.Micro](https://github.com/Caliburn-Micro/Caliburn.Micro)
- [Microsoft AppCenter SDK](https://github.com/Microsoft/AppCenter-SDK-DotNet)
- [Microsoft Store Services SDK](https://marketplace.visualstudio.com/items?itemName=AdMediator.MicrosoftStoreServicesSDK)
- [Microsoft Win 2D](https://github.com/Microsoft/Win2D)
- [MVVMLight](https://github.com/lbugnion/mvvmlight)
- [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)
- [Prism](https://github.com/PrismLibrary/Prism)
- [Telerik UI For UWP](https://github.com/telerik/UI-For-UWP)
- [Windows Azure Messaging Managed](https://www.nuget.org/packages/WindowsAzure.Messaging.Managed)
- [Windows Community Toolkit](https://github.com/Microsoft/WindowsCommunityToolkit)
