## WinUI 3 Desktop Apps

We're looking into adding WinUI 3 Desktop App templates to Windows Template Studio.

Our templates for WinUI 3 Desktop Apps are currently in preview as WinUI3 itself. You'll find them soon in our [dev-nightly version](./../getting-started-extension.md#nightly--pre-release-feeds-for-windows-template-studio).

We would love to get your feedback on our [tracking issue for WinUI3 templates](https://github.com/microsoft/WindowsTemplateStudio/issues/3810).

If you miss anything or find an issue that is not mentioned in the known issues section please file a new issue.

### Prerequisites

1. Ensure that your development computer has Windows 10, version 1803 (build 17134), or newer installed.

2. Requires Visual Studio 2019, version 16.7.2 and the following workloads:
    - .NET Desktop Development
    - Universal Windows Platform development

3. Make sure you enable .NET previews within Visual Studio:
     Go to Tools > Options > Preview Features > Select "Use previews of the .NET Core SDK (requires restart)".

4. Install both x64 and x86 versions of .NET 5 Preview 5. Note that .NET 5 Preview 5 is currently the only supported .NET 5 preview for WinUI 3:
    - x64: https://aka.ms/dotnet/net5/preview5/Sdk/dotnet-sdk-win-x64.exe
    - x86: https://aka.ms/dotnet/net5/preview5/Sdk/dotnet-sdk-win-x86.exe

### Known issues:
- [Dark/Light Theme issue](https://github.com/microsoft/microsoft-ui-xaml/issues/3384)
- [Windows Default theme issue](https://github.com/microsoft/microsoft-ui-xaml/issues/3385)
- [Master/Detail Issue with WinUI](https://github.com/windows-toolkit/WindowsCommunityToolkit/issues/3433)
- [Localized Ressource issue](https://github.com/microsoft/microsoft-ui-xaml/issues/2602)

### Additional docs:
- [Windows UI Library 3 Preview 2 (July 2020)](https://docs.microsoft.com/es-es/windows/apps/winui/winui3/)
- [Windows UI Library on GitHub](https://github.com/Microsoft/microsoft-ui-xaml)