Forced Login ajoute l'authentification utilisateur grâce à Azure AD et [Microsoft.Identity.Client](https://www.nuget.org/packages/Microsoft.Identity.Client) (MSAL).
L'accès à votre application est limité aux utilisateurs connectés et autorisés. Si une connexion interactive est requise, l'utilisateur est redirigé vers une LoginPage avant que la boîte de dialogue interactive ne s'affiche. La même page s'affiche après la déconnexion.

L'application comprend une demande à Microsoft Graph dans le but d'afficher les informations et la photo de l'utilisateur dans le NavigationPane et la SettingsPage.  Par défaut, cette fonctionnalité fonctionne avec les comptes de n'importe quel répertoire organisationnel ainsi qu'avec les comptes personnels Microsoft (par exemple : Skype, Xbox, Outlook.com). Elle permet d'exclure des comptes personnels Microsoft, de limiter l'accès à un répertoire spécifique ou d'utiliser Integrated Windows Authentication.

[En savoir plus sur la plateforme Microsoft Identity.](https://docs.microsoft.com/azure/active-directory/develop/v2-overview)
