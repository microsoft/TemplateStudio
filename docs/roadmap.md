# Roadmap

Our priorities for this year (2022) include:

* Supporting Visual Studio 2022
* Simplifying extension installation and rebranding
* Updating WinUI and WPF templates to .NET 6.0
* Updating WinUI templates to Windows App SDK 1.0
* Adding features to templates based on priority and demand
* Improving documentation to encourage community contributions
* Refactoring backend infrastructure

## Supporting Visual Studio 2022

Supporting both Visual Studio 2022 and Visual Studio 2019 requires separate extensions with separate IDs and separate Marketplace listings. At the same time, .NET 6.0 requires Visual Studio 2022, and .NET 5.0 and .NET Core 3.1 will go out of support this year per the [.NET support lifecycle](https://github.com/dotnet/core/blob/main/releases.md#supported-net-versions).

After evaluating options for supporting both versions, assessing the cost of ongoing maintenance and support, and considering the [.NET support lifecycle](https://github.com/dotnet/core/blob/main/releases.md#supported-net-versions), we've concluded that we will not be updating the existing Visual Studio 2019 extension going forward. This repository will shift focus to producing only Visual Studio 2022 extensions.

The existing Visual Studio 2019 extension will remain in place at least until the end of 2022. At that point, all .NET runtimes used by desktop projects built with the extension will be out of support, and as noted earlier, updating those projects to .NET 6.0 will require an update to Visual Studio 2022. At that point, it is likely that we will retire the Visual Studio 2019 extension. This will also enable us to retire some backend services that we will no longer be using in the Visual Studio 2022 extensions.

Note: UWP apps built with .NET Native are still supported in Visual Studio 2022 and will still be supported in the Visual Studio 2022 extension.

In order to expedite publishing the Visual Studio 2022 extensions, we will publish them initially as English-only. We are onboarding the project to a new localization service in parallel and will add back localization as part of that process in future updates.

## Simplifying extension installation and rebranding

Windows Template Studio today is a single extension that handles all supported UI frameworks and languages. One implication of this is that it requires all possible dependent Visual Studio workloads to be installed to create any project, even if you otherwise do not need all of those workloads. This slows down installation of the extension and can consume unnecessary disk space.

To address this, we will be splitting up the single extension into multiple extensions:

* Template Studio for WinUI (C#)
* Template Studio for WinUI (C++)
* Template Studio for WPF
* Template Studio for UWP

By splitting up the extensions like this, you will only need to install the workloads that you need to create the projects you care about.

As part of this change, we will also be removing "Windows" from the extension names as shown above. We have seen requests for Windows Template Studio support for other frameworks like [MAUI](https://github.com/microsoft/WindowsTemplateStudio/issues/4117) and [Uno](https://github.com/microsoft/WindowsTemplateStudio/issues/2658) that are not Windows-specific. While we're not committing to supporting these frameworks right now, removing "Windows" from the extension name better facilitates that in the future, and the additional "for &lt;FRAMEWORK&gt;" qualifier we are adding to the name by splitting up the extension makes it clear what platform you are targeting. To align with the new naming convention, we have renamed the repository from microsoft/WindowsTemplateStudio to microsoft/TemplateStudio. Links to the old repository name should redirect to the new name.

## Updating WinUI and WPF templates to .NET 6.0

With the decision to support only Visual Studio 2022 going forward, and with .NET 5.0 going out of support in May, we will be updating all of the WinUI and WPF templates to .NET 6.0.

Projects created with the existing Visual Studio 2019 extension will remain unchanged.

## Updating WinUI templates to Windows App SDK 1.0

We will be updating the WinUI templates to the Windows App SDK 1.0 release.

## Adding features to templates based on priority and demand

Our primary focus will be on adding features to the WinUI templates going forward. The first feature we will be adding is making MSIX packaging optional so that unpackaged projects can be created. As part of this change, we will also be migrating the packaged templates to use the new [single-project packaging solution](https://docs.microsoft.com/en-us/windows/apps/windows-app-sdk/single-project-msix?tabs=csharp).

Additional features will be triaged and prioritized based on demand using telemetry from the WPF and UWP templates as a starting point.

## Improving documentation to encourage community contributions

Community contributions can help scale out to more features faster than any single team can produce on their own. We will be simplifying the codebase and improving the documentation to encourage and facilitate more community contributions.

## Refactoring backend infrastructure

We will be refactoring and optimizing our CI pipelines to support the new multi-extension project structure. We are also onboarding the project to an automated localization service so that we can continue to provide localization with minimal cost and overhead going forward.

DISCLAIMER: The roadmap is subject to change.

## Past releases
- [4.1 - June 2021](https://github.com/microsoft/WindowsTemplateStudio/milestone/32)
- [4.0 - June 2021](https://github.com/microsoft/WindowsTemplateStudio/milestone/31)
- [3.10 - May 2021](https://github.com/Microsoft/WindowsTemplateStudio/issues?q=is%3Aissue+milestone%3A3.10)
- [3.9 - December 2020](https://github.com/Microsoft/WindowsTemplateStudio/issues?utf8=%E2%9C%93&q=is%3Aissue+milestone%3A3.9)
- [3.8 - September 2020](https://github.com/Microsoft/WindowsTemplateStudio/issues?utf8=%E2%9C%93&q=is%3Aissue+milestone%3A3.8)
- [3.7 - July 2020](https://github.com/Microsoft/WindowsTemplateStudio/issues?utf8=%E2%9C%93&q=is%3Aissue+milestone%3A3.7)
- [3.6 - May 2020](https://github.com/Microsoft/WindowsTemplateStudio/issues?utf8=%E2%9C%93&q=is%3Aissue+milestone%3A3.6)
- [3.5 - November 2019](https://github.com/Microsoft/WindowsTemplateStudio/issues?utf8=%E2%9C%93&q=is%3Aissue+milestone%3A3.5)
- [3.4 - September 2019](https://github.com/Microsoft/WindowsTemplateStudio/issues?utf8=%E2%9C%93&q=is%3Aissue+milestone%3A%22Hotfix+3.4%22)
- [3.3 - July 2019](https://github.com/Microsoft/WindowsTemplateStudio/issues?utf8=%E2%9C%93&q=is%3Aissue+milestone%3A3.3)
- [3.2 - April 2019](https://github.com/Microsoft/WindowsTemplateStudio/issues?utf8=%E2%9C%93&q=is%3Aissue+milestone%3A3.2)
- [3.1 - March 2019](https://github.com/Microsoft/WindowsTemplateStudio/issues?utf8=%E2%9C%93&q=is%3Aissue+milestone%3A3.1)
- [3.0 - January 2019](https://github.com/Microsoft/WindowsTemplateStudio/issues?utf8=%E2%9C%93&q=is%3Aissue+milestone%3A3.0)
- [2.5 - November 2018](https://github.com/Microsoft/WindowsTemplateStudio/issues?utf8=%E2%9C%93&q=is%3Aissue+milestone%3A2.5)
- [2.4 - September 2018](https://github.com/Microsoft/WindowsTemplateStudio/issues?utf8=%E2%9C%93&q=is%3Aissue+milestone%3A2.4)
- [2.3 - July 2018](https://github.com/Microsoft/WindowsTemplateStudio/issues?utf8=%E2%9C%93&q=is%3Aissue+milestone%3A2.3)
- [2.2 - June 2018](https://github.com/Microsoft/WindowsTemplateStudio/issues?utf8=%E2%9C%93&q=is%3Aissue+milestone%3A2.2)
- [2.1 - May 2018](https://github.com/Microsoft/WindowsTemplateStudio/issues?utf8=%E2%9C%93&q=is%3Aissue+milestone%3A2.1)
- [2.0 - April 2018](https://github.com/Microsoft/WindowsTemplateStudio/issues?utf8=%E2%9C%93&q=is%3Aissue+milestone%3A2.0)
- [1.7 - January 2018](https://github.com/Microsoft/WindowsTemplateStudio/issues?utf8=%E2%9C%93&q=is%3Aissue+milestone%3A1.7)
- [1.6 - December 2017](https://github.com/Microsoft/WindowsTemplateStudio/issues?utf8=%E2%9C%93&q=is%3Aissue+milestone%3A1.6)
- [1.5 - November 2017](https://github.com/Microsoft/WindowsTemplateStudio/issues?utf8=%E2%9C%93&q=is%3Aissue+milestone%3A1.5)
- [1.4 - October 2017](https://github.com/Microsoft/WindowsTemplateStudio/issues?utf8=%E2%9C%93&q=is%3Aissue+milestone%3A1.4)
- [1.3 - September 2017](https://github.com/Microsoft/WindowsTemplateStudio/issues?utf8=%E2%9C%93&q=is%3Aissue+milestone%3A1.3)
- [1.2 - July 2017](https://github.com/Microsoft/WindowsTemplateStudio/issues?utf8=%E2%9C%93&q=is%3Aissue%20milestone%3A1.2)
- [1.1 - June 2017](https://github.com/Microsoft/WindowsTemplateStudio/issues?utf8=%E2%9C%93&q=is%3Aissue%20milestone%3A1.1)
- [1.0.17137.1 (Template Update Release)](https://github.com/Microsoft/WindowsTemplateStudio/issues?utf8=%E2%9C%93&q=is%3Aissue+milestone%3A%221.01+-+Critical+Bug+Fixes%22)
- [1.0.17142.1 (Template Update Release)](https://github.com/Microsoft/WindowsTemplateStudio/issuesutf8=%E2%9C%93&?q=is%3Aissue+milestone%3A%221.01+-+Critical+Bug+Fixes%22)
- [1.0 - May 11th, 2017](https://github.com/Microsoft/WindowsTemplateStudio/issues?utf8=%E2%9C%93&q=is%3Aissue+milestone%3A1.0)
- [0.5 - April 30th, 2017](https://github.com/Microsoft/WindowsTemplateStudio/issues?utf8=%E2%9C%93&q=is%3Aissue+milestone%3A0.5)
