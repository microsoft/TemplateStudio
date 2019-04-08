Forced Login は、Azure AD と [Microsoft.Identity.Client](https://www.nuget.org/packages/Microsoft.Identity.Client) (MSAL) を使用するユーザー認証を追加します。
アプリへのアクセスは、許可されているログイン済みのユーザーに限定されます。対話型ログインが必要な場合は、ユーザーは LoginPage にリダイレクトされ、対話型ダイアログが表示されます。ログアウト後も、これと同じページが表示されます。

アプリケーションには、NavigationPane と SettingsPage にユーザー情報とユーザーの写真を表示する、Microsoft Graph への呼び出しが含まれています。この機能は、既定では、任意の組織ディレクトリに登録されているアカウントと個人用の Microsoft アカウント (Skype、Xbox、Outlook.com など) で使用できますが、個人用の Microsoft アカウントを除外して、特定のディレクトリにのみアクセスを制限するか、Windows 統合認証を使用するように設定することもできます。

[Microsoft Identity プラットフォームの詳細をご覧ください。](https://docs.microsoft.com/azure/active-directory/develop/v2-overview)
