Optional Login aggiunge l'autenticazione utente tramite Azure AD e [Microsoft.Identity.Client](https://www.nuget.org/packages/Microsoft.Identity.Client) (MSAL). 
Questo stile consente di combinare contenuto con e senza restrizioni. Il contenuto con restrizioni è mostrato solo agli utenti connessi e autorizzati.
L'applicazione include una chiamata a Microsoft Graph, al fine di mostrare la foto e le informazioni dell'utente in NavigationPane e SettingsPage.

Per impostazione predefinita, questa funzionalità viene utilizzata con gli account in una directory organizzativa e con gli account Microsoft personali, ad esempio Skype, Xbox e Outlook.com, e consente di escludere gli account Microsoft personali, limitare l'accesso a una directory specifica o usare l'autenticazione integrata.

[Scopri di più su Microsoft Identity Platform.](https://docs.microsoft.com/azure/active-directory/develop/v2-overview)
