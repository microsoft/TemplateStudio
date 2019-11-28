# Optional Login

Optional login adds user authentication using Azure AD and [Microsoft.Identity.Client](https://www.nuget.org/packages/Microsoft.Identity.Client) (MSAL).
This style allows to combine unrestricted and restricted content. Restricted content is only shown to logged in and authorized users.
The application includes a call to the Microsoft Graph to show user info and photo on the NavigationPane and the SettingsPage.

By default this features works with Accounts in any organizational directory and personal Microsoft accounts (e.g. Skype, Xbox, Outlook.com) and gives the option to exclude personal Microsoft accounts, limit access to a specific directory or use integrated auth.

[Learn more about using identity in WinTS generated apps](./identity.md).

[Learn more about Microsoft Identity platform.](https://docs.microsoft.com/azure/active-directory/develop/v2-overview)
