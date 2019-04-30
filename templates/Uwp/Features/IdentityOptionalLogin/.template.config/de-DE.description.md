Die optionale Anmeldung fügt die Benutzerauthentifizierung über Azure AD und [Microsoft.Identity.Client](https://www.nuget.org/packages/Microsoft.Identity.Client) (MSAL) hinzu.
Diese Anmeldungsmethode ermöglicht die Kombination von nicht eingeschränkten und eingeschränkten Inhalten. Eingeschränkte Inhalte werden nur für angemeldete und autorisierte Benutzer angezeigt.
Die Anwendung enthält einen Aufruf von Microsoft Graph zur Anzeige von Benutzerinformationen und -foto im NavigationPane und auf der SettingsPage.

Standardmäßig funktioniert dieses Feature für Konten in allen Organisationsverzeichnissen und für persönliche Microsoft-Konten (z. B. Skype, Xbox, Outlook.com). Sie ermöglicht den Ausschluss von persönlichen Microsoft-Konten, die Einschränkung des Zugriffs auf ein bestimmtes Verzeichnis oder die Verwendung der integrierten Authentifizierung.

[Erfahren Sie mehr über die Microsoft Identity-Plattform.](https://docs.microsoft.com/azure/active-directory/develop/v2-overview)
