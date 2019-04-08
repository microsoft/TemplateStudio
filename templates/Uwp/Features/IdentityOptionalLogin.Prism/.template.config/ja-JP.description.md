Optional login は、Azure AD と [Microsoft.Identity.Client](https://www.nuget.org/packages/Microsoft.Identity.Client) (MSAL) を使用するユーザー認証を追加します。
このスタイルを使用すると、制限なしのコンテンツと制限付きのコンテンツを組み合わせることができます。制限付きのコンテンツは、ログインした、権限のあるユーザーにしか表示されません。
アプリケーションには、NavigationPane と SettingsPage にユーザー情報とユーザーの写真を表示する、Microsoft Graph への呼び出しが含まれています。

この機能は、既定では、任意の組織ディレクトリに登録されているアカウントと個人用の Microsoft アカウント (Skype、Xbox、Outlook.com など) で使用できますが、個人用の Microsoft アカウントを除外して、特定のディレクトリにアクセスを制限することも、統合認証を使用することもできます。

[Microsoft Identity プラットフォームの詳細をご覧ください。](https://docs.microsoft.com/azure/active-directory/develop/v2-overview)
