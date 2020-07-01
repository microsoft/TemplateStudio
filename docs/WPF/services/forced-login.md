# Forced Login

Forced Login adds user authentication using Azure AD and [Microsoft.Identity.Client](https://www.nuget.org/packages/Microsoft.Identity.Client) (MSAL).
Access to your app is restricted to logged in and authorized users. If interactive login is required, the user is redirected to a LoginPage before showing the interactive dialog. The same page is shown after logging out.

The application includes a call to the Microsoft Graph to show user info and photo on the NavigationPane and the SettingsPage.  By default this features works with accounts in any organizational directory and personal Microsoft accounts (e.g. Skype, Xbox, Outlook.com) and gives the option to exclude personal Microsoft accounts, limit access to a specific directory or use windows integrated auth.

[Learn more about using identity in WinTS generated apps](./identity.md).

[Learn more about Microsoft Identity platform.](https://docs.microsoft.com/azure/active-directory/develop/v2-overview)
