„Setting storage“ ist eine Klasse, mit der Sie das Speichern Ihrer Anwendungsdaten vereinfachen können.  Sie behandelt das Laden, Speichern und Serialisieren Ihrer Daten sowie den einfachen Zugriff auf die Daten Ihrer Anwendung.

Dies sind die Haupttypen von App-Daten:

* Local: Diese Daten werden auf dem Gerät gespeichert und in der Cloud gesichert. Sie sind über Aktualisierungen hinweg persistent.
* LocalCache: Diese Daten befinden sich auf dem aktuellen Gerät, sind persistent und werden nicht gesichert. Sie sind über Aktualisierungen hinweg persistent.
* SharedLocal: Diese Daten sind über alle App-Benutzer hinweg persistent.
* Roaming: Diese Daten sind auf allen Geräten vorhanden, auf denen der Benutzer die App installiert hat.
* Temporary: Diese Daten können jederzeit vom System gelöscht werden.

Weitere Informationen zum Speichern finden Sie auf [docs.microsoft.com](https://docs.microsoft.com/de-de/uwp/api/windows.storage.applicationdata).
