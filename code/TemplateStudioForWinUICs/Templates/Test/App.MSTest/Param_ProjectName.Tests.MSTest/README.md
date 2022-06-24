# MSTests for Template Studio

This guide will demonstrate using dependency injection and service-mocking to test your page View Models.

## Unpackaged App
Settings: <ProjectPriFileName>resources.pri</ProjectPriFileName> to the first PropertyGroup
(will be handled by post action)

## Packaged App

    <PackageReference Include="Microsoft.TestPlatform.TestHost">
    <Version>17.2.0</Version>
    <ExcludeAssets>build</ExcludeAssets>
    </PackageReference>

    NEED TO FIND RESOURCE FILE FIX


<figcaption>VS New Project Dialog</figcaption>

![VS New Project screenshot](./docs/resources/getting-started/VS-NewProject-WinUI.png)

<figcaption>Template Studio for WinUI (C#)</figcaption>

![Template Studio for WinUI screenshot](./docs/resources/getting-started/Wizard-ProjectTypes-WinUI.png)

<figcaption>Template Studio for WPF</figcaption>

![Template Studio for WPF screenshot](./docs/resources/getting-started/Wizard-ProjectTypes-WPF.png)

<figcaption>Template Studio for UWP</figcaption>

![Template Studio for UWP screenshot](./docs/resources/getting-started/Wizard-ProjectTypes-UWP.png)

## Build Status

|         |CI                |Nightly           |
|:--------|:----------------:|:----------------:|
|WinUI (C#)|[![Build Status](https://winappstudio.visualstudio.com/WTS/_apis/build/status/Template%20Studio/CI%20-%20WinUICs?branchName=main)](https://winappstudio.visualstudio.com/WTS/_build/latest?definitionId=196&branchName=main)|[![Build Status](https://winappstudio.visualstudio.com/WTS/_apis/build/status/Template%20Studio/Nightly%20-%20WinUICs?branchName=main)](https://winappstudio.visualstudio.com/WTS/_build/latest?definitionId=201&branchName=main)|
|WinUI (C++)|[![Build Status](https://winappstudio.visualstudio.com/WTS/_apis/build/status/Template%20Studio/CI%20-%20WinUICpp?branchName=main)](https://winappstudio.visualstudio.com/WTS/_build/latest?definitionId=197&branchName=main)|[![Build Status](https://winappstudio.visualstudio.com/WTS/_apis/build/status/Template%20Studio/Nightly%20-%20WinUICpp?branchName=main)](https://winappstudio.visualstudio.com/WTS/_build/latest?definitionId=202&branchName=main)|
|WPF|[![Build Status](https://winappstudio.visualstudio.com/WTS/_apis/build/status/Template%20Studio/CI%20-%20WPF?branchName=main)](https://winappstudio.visualstudio.com/WTS/_build/latest?definitionId=198&branchName=main)|[![Build Status](https://winappstudio.visualstudio.com/WTS/_apis/build/status/Template%20Studio/Nightly%20-%20WPF?branchName=main)](https://winappstudio.visualstudio.com/WTS/_build/latest?definitionId=203&branchName=main)|
|UWP|[![Build Status](https://winappstudio.visualstudio.com/WTS/_apis/build/status/Template%20Studio/CI%20-%20UWP?branchName=main)](https://winappstudio.visualstudio.com/WTS/_build/latest?definitionId=199&branchName=main)|[![Build Status](https://winappstudio.visualstudio.com/WTS/_apis/build/status/Template%20Studio/Nightly%20-%20UWP?branchName=main)](https://winappstudio.visualstudio.com/WTS/_build/latest?definitionId=204&branchName=main)|
|Shared|[![Build Status](https://winappstudio.visualstudio.com/WTS/_apis/build/status/Template%20Studio/CI%20-%20Shared?branchName=main)](https://winappstudio.visualstudio.com/WTS/_build/latest?definitionId=200&branchName=main)|[![Build Status](https://winappstudio.visualstudio.com/WTS/_apis/build/status/Template%20Studio/Nightly%20-%20Shared?branchName=main)](https://winappstudio.visualstudio.com/WTS/_build/latest?definitionId=205&branchName=main)|

- CI: Runs for all PRs and integrations and performs basic validation. Executes all tests in the Minimum* Groups.
- Nightly: Runs on a nightly schedule and generates and builds all combinations of project templates in addition to performing more exhaustive tests than CI. Executes all tests in the Build* Groups.

## Features

Template Studio approaches app creation using the following six attribute sets:

### **Project type**

First, how do you want your app's UI navigation to behave?

- **WinUI 3**: *[Blank](./docs/WinUI/projectTypes/blank.md)*, *[Navigation Pane](./docs/WinUI/projectTypes/navigationpane.md)*, *[Menu Bar](./docs/WinUI/projectTypes/menubar.md)*.

- **WPF**: *[Blank](./docs/WPF/projectTypes/blank.md)*, *[Navigation Pane](./docs/WPF/projectTypes/navigationpane.md)*, *[Menu Bar](./docs/WPF/projectTypes/menubar.md)* and a *[Ribbon](./docs/WPF/projectTypes/ribbon.md)*.

- **UWP**: *[Blank](./docs/UWP/projectTypes/blank.md)*, *[Navigation Pane](./docs/UWP/projectTypes/navigationpane.md)*, *[Horizontal Navigation Pane](./docs/UWP/projectTypes/horizontalnavigationpane.md)*, and a *[Menu Bar](./docs/UWP/projectTypes/menubar.md)*.

### **App design pattern**

Next, what coding pattern do you want to use in your project.

- **WinUI 3**: *[MVVM Toolkit](./docs/WinUI/frameworks/mvvmtoolkit.md)*.

- **WPF**: *[MVVM Toolkit](./docs/WPF/frameworks/mvvmtoolkit.md)*, *[Code Behind](./docs/WPF/frameworks/codebehind.md)*, and *[Prism](./docs/WPF/frameworks/prism.md)*.

- **UWP**: *[MVVM Toolkit](./docs/UWP/frameworks/mvvmtoolkit.md)*, *[Code Behind](./docs/UWP/frameworks/codebehind.md)*, and *[Prism](./docs/UWP/frameworks/prism.md)*.

### **App pages**

To accelerate app creation, we provide a number of app page templates you can use to add common UI pages into your new app. We currently include everything from a *blank page*, to common layouts (*e.g., list/detail, web view*), to pages that implement common patterns (*e.g., app settings*). Use the wizard to add as many of each page as you need, providing a name for each one, and we'll generate them for you.

### **Features**

Specify which capabilities you want to use in your app, and we'll build out the framework for the features into your app, tagging `TODO` items. Here you can add features that enable your app to interact with storage, notifications, layout theming, etc.

### **Services**

Connect to data services, setup cloud connected services for your application, and enforce rules with the [XAML Styler](https://github.com/Xavalon/XamlStyler) extension.

### **Automated Tests**

Lastly, you can include test projects for your application to run unit tests or use Selenium-like UI test automation.

After selecting the items wanted in your app, you can extend the generated code on [UWP](./docs/UWP/getting-started-endusers.md), [WPF](./docs/WPF/getting-started-endusers.md) or [WinUI 3](./docs/WinUI/readme.md).

## Documentation

- [Using TS to build WinUI 3 apps](./docs/WinUI/readme.md)
- [Using TS to build WPF apps](./docs/WPF/getting-started-endusers.md)
- [Using TS to build UWP apps](./docs/UWP/getting-started-endusers.md)

## Principles

1. Generated templates will be kept simple.
2. Generated templates are a starting point, not a completed application.
3. Generated templates must be able to compile and run once generated.
4. Generated templates should work on all device families.
5. Templates should have comments to aid developers. This includes links to signup pages for keys, MSDN, blogs and how-to's.  All guidance provide should be validated from either the framework/SDK/library’s creator.
6. All UWP features will be supported for two most recent RTM Windows 10 Updates. Those supported releases are Windows 10 May 2020 Update (version 2004) and Windows 10 May 2019 Update (version 1903).
7. Templates released in production will try to adhere to the design language used in the current release of Windows 10.
8. Code should follow [.NET Core coding style](https://github.com/dotnet/runtime/blob/main/docs/coding-guidelines/coding-style.md).

This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behavior in our community.
For more information see the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct).

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md).

## Privacy Statement

The extension logs [basic telemetry](./docs/telemetry.md) regarding what is selected in the wizard. Our [Telemetry Data](./docs/telemetryData.md) page has the trends from the telemetry. Please read the [Microsoft privacy statement](http://go.microsoft.com/fwlink/?LinkId=521839) for more information.

## .NET Foundation

This project is supported by the [.NET Foundation](https://dotnetfoundation.org).

## Projects we like and collaborate with

- [Web Template Studio](https://github.com/Microsoft/WebTemplateStudio)
- [Rapid Xaml Toolkit](https://github.com/Microsoft/Rapid-XAML-Toolkit)
- [Windows Community Toolkit](https://github.com/Microsoft/WindowsCommunityToolkit)
- [Windows UI](https://github.com/microsoft/microsoft-ui-xaml)
- [Windows App SDK](https://github.com/microsoft/WindowsAppSDK)
- [Fluent XAML Theme Editor](https://github.com/Microsoft/fluent-xaml-theme-editor)
- [XAML Styler](https://github.com/Xavalon/XamlStyler)

## Frameworks and libraries in generated code not created by our team

### Frameworks
- [MVVM Toolkit](https://aka.ms/mvvmtoolkit)
- [Prism](https://github.com/PrismLibrary/Prism)
- [Caliburn.Micro](https://github.com/Caliburn-Micro/Caliburn.Micro)
- [MVVMLight](https://github.com/lbugnion/mvvmlight)

### Libraries

- [AdaptiveCards](https://adaptivecards.io/)
- [Appium.WebDriver](https://github.com/appium/appium-dotnet-driver)
- [MahApps.Metro](https://github.com/MahApps/MahApps.Metro)
- [Microsoft AppCenter SDK](https://github.com/Microsoft/AppCenter-SDK-DotNet)
- [Microsoft Authentication Library (MSAL)](https://github.com/AzureAD/microsoft-authentication-library-for-dotnet)
- [Microsoft Store Services SDK](https://marketplace.visualstudio.com/items?itemName=AdMediator.MicrosoftStoreServicesSDK)
- [Microsoft Win2D](https://github.com/Microsoft/Win2D)
- [MSTest V2](https://github.com/microsoft/testfx)
- [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)
- [NUnit](https://nunit.org/)
- [Telerik UI For UWP](https://github.com/telerik/UI-For-UWP)
- [Windows Azure Messaging Managed](https://www.nuget.org/packages/WindowsAzure.Messaging.Managed)
- [Windows Community Toolkit](https://github.com/Microsoft/WindowsCommunityToolkit)
- [Windows UI Library](https://github.com/Microsoft/microsoft-ui-xaml)
- [xunit](https://github.com/xunit/xunit)
