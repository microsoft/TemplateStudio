此页包括使用 **WindowsXamlHost** 控件（来自 **Windows 社区工具包**）的自定义用户控制来托管 UWP 控件。

此页向解决方案添加了两个项目：

- **UWP 库**，该库将承载将在 WPF 应用中呈现的 UWP 控件。

- **UWP 应用项目**，这是使用 XAML 岛托管 UWP 控件所必需的，永远不会显示。

您可以在使用 XAML 群岛的"https://docs.microsoft.com/windows/apps/desktop/modernize/host-custom-control-with-xaml-islands