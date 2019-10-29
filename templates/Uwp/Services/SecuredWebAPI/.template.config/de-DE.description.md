Diese Vorlage fügt eine ASP.NET Core Web-API hinzu, die das an die UWP-Anwendung übergebene JWToken validiert.

Der Schutz stellt sicher, dass die API nur von folgenden Entitäten aufgerufen werden kann:

* Anwendungen, die die korrekten Bereiche im Namen von Benutzern anfordern.
* Benutzer, die die korrekten Anwendungsrollen besitzen.

