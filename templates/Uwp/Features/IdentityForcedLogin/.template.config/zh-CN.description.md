Forced Login 添加了使用 Azure AD 和 [Microsoft.Identity.Client](https://www.nuget.org/packages/Microsoft.Identity.Client) (MSAL) 的用户身份验证。
仅授权用户经登录后才能访问你的应用程序。如要求交互式登录，系统会在显示交互式对话框之前将用户重定向至 LoginPage。注销后也会显示同一页面。

此应用程序可调用 Microsoft Graph，以在 NavigationPane 和 SettingsPage 上显示用户信息和照片。默认情况下，此功能适用于任何组织目录中的帐户和 Microsoft 个人帐户（例如 Skype、Xbox、Outlook.com 帐户），并提供排除个人 Microsoft 帐户、限制对特定路径的访问或使用 Windows 集成身份验证的选项。

[了解有关 Microsoft Identity 平台的更多信息。](https://docs.microsoft.com/azure/active-directory/develop/v2-overview)
