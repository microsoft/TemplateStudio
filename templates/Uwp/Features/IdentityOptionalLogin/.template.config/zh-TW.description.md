選擇性登入會使用 Azure AD 和 [Microsoft.Identity.Client](https://www.nuget.org/packages/Microsoft.Identity.Client) (MSAL) 新增使用者驗證。
此樣式可結合非限制性和限制性內容。限制性內容只會顯示給已登入且授權的使用者。
此應用程式包含對 Microsoft Graph 的呼叫，以便在 NavigationPane 和 SettingsPage 上顯示使用者資訊和相片。

根據預設，此功能適用於任何組織目錄中的帳戶及個人 Microsoft 帳戶 (例如 Skype、Xbox、Outlook.com)，而且可讓您選擇排除個人 Microsoft 帳戶、限制特定目錄的存取或使用整合式驗證。

[深入了解 Microsoft Identity 平台。](https://docs.microsoft.com/azure/active-directory/develop/v2-overview)
