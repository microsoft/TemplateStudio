此頁包括使用 **WindowsXamlHost** 控制件(來自 **Windows 社區工具組**)的自訂使用者控制來託管 UWP 控件。

此頁向解決方案新增了兩個專案:

- **UWP 庫**,該庫將承載將在 WPF 應用中呈現的 UWP 控件。

- **UWP 應用專案**,這是使用 XAML 島託管 UWP 控件所必需的,永遠不會顯示。

您可以在使用 XAML 群島的"HTTPs://docs.microsoft.com/windows/apps/desktop/modernize/host-custom-control-with-xaml-islands。