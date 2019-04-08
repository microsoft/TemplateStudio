Forced Login fügt eine Benutzerauthentifizierung über Azure AD und [Microsoft.Identity.Client](https://www.nuget.org/packages/Microsoft.Identity.Client) (MSAL) hinzu. 
Der Zugriff auf Ihre App ist auf angemeldete und autorisierte Benutzer eingeschränkt. Wenn eine interaktive Anmeldung erforderlich ist, wird der Benutzer vor der Anzeige des Dialogfensters für die interaktive Anmeldung zu einer Anmeldeseite umgeleitet. Diese Seite wird auch nach der Abmeldung angezeigt.

Die Anwendung enthält einen Aufruf von Microsoft Graph zur Anzeige von Benutzerinformationen und -foto im NavigationPane und auf der SettingsPage.  Standardmäßig funktioniert dieses Feature für Konten in allen Organisationsverzeichnissen und für persönliche Microsoft-Konten (z. B. Skype, Xbox, Outlook.com). Sie ermöglicht den Ausschluss von persönlichen Microsoft-Konten, die Einschränkung des Zugriffs auf ein bestimmtes Verzeichnis oder die Verwendung der in Windows integrierten Authentifizierung.

[Erfahren Sie mehr über die Microsoft Identity-Plattform.](https://docs.microsoft.com/azure/active-directory/develop/v2-overview)
