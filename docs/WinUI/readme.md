## WinUI 3 Desktop Apps

We're looking into adding WinUI 3 Desktop App templates to Windows Template Studio.

Our templates for WinUI 3 Desktop Apps are currently in preview as WinUI3 itself. You can try them out in our [dev-nightly version](./../getting-started-extension.md#nightly--pre-release-feeds-for-windows-template-studio).

We would love to get your feedback on our [tracking issue for WinUI3 templates](https://github.com/microsoft/WindowsTemplateStudio/issues/3810).

If you miss anything or find an issue that is not mentioned in the known issues section please file a new issue.

### Prerequisites

1. Ensure that your development computer has Windows 10, version 1803 (build 17134), or newer installed.

2. Install [Visual Studio 2019, version 16.9 Preview](https://visualstudio.microsoft.com/vs/preview/). You must include the following workloads when installing Visual Studio:
   - .NET Desktop Development (this also installs .NET 5)
   - Universal Windows Platform development

3. Make sure your system has a NuGet package source enabled for nuget.org. For more information, see [Common NuGet configurations](https://docs.microsoft.com/en-us/nuget/consume-packages/configuring-nuget-behavior)

4. Add a new Package source to the Windows Community DevOps feed: 
   - Go to Tools -> NuGet Package Manager -> Package Manager Settings -> Package Sources
   - Add a new Package Source for https://pkgs.dev.azure.com/dotnet/WindowsCommunityToolkit/_packaging/WindowsCommunityToolkit-WinUI3/nuget/v3/index.json
   - Confirm by clicking update, then Ok


### Known issues:
- [Windows Default theme issue](https://github.com/microsoft/microsoft-ui-xaml/issues/3385)
- [Dark/Light theme issue](https://github.com/microsoft/microsoft-ui-xaml/issues/3384)
- [Master/Detail Issue with WinUI](https://github.com/windows-toolkit/WindowsCommunityToolkit/issues/3433)
- [Localized tooltips issue](https://github.com/microsoft/microsoft-ui-xaml/issues/3649)
- [WebView2 debugging exception](https://github.com/microsoft/microsoft-ui-xaml/issues/4206)

### Additional docs:
- [Windows UI Library 3 Preview 4 (November 2020)](https://docs.microsoft.com/windows/apps/winui/winui3/)
- [Windows UI Library on GitHub](https://github.com/Microsoft/microsoft-ui-xaml)
- [Windows Community Toolkit 8.0.0-preview4 for WinUI 3 Preview 4](https://github.com/windows-toolkit/WindowsCommunityToolkit/issues/3295)
