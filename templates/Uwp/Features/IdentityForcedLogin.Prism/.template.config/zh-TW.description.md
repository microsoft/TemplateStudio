Forced Login 會使用 Azure AD 和 [Microsoft.Identity.Client](https://www.nuget.org/packages/Microsoft.Identity.Client) (MSAL) 新增使用者驗證。
僅限已登入且授權的使用者才可存取您的應用程式。如果需要互動式登入，系統會將使用者重新導向 LoginPage，然後顯示互動式對話方塊。在登出後會顯示相同的頁面。

此應用程式包含對 Microsoft Graph 的呼叫，以便在 NavigationPane 和 SettingsPage 上顯示使用者資訊和相片。根據預設，此功能適用於任何組織目錄中的帳戶及個人 Microsoft 帳戶 (例如 Skype、Xbox、Outlook.com)，而且可讓您選擇排除個人 Microsoft 帳戶、限制特定目錄的存取或使用 Windows 整合式驗證。

[深入了解 Microsoft Identity 平台。](https://docs.microsoft.com/azure/active-directory/develop/v2-overview)
