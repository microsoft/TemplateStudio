Szablon Forced Login dodaje uwierzytelnianie użytkowników przy użyciu usługi Azure AD i biblioteki [Microsoft.Identity.Client](https://www.nuget.org/packages/Microsoft.Identity.Client) (MSAL). 
Dostęp do aplikacji zostaje ograniczony do zalogowanych i autoryzowanych użytkowników. Jeśli wymagane jest logowanie interakcyjne, użytkownik zostaje przekierowany na stronę LoginPage, zanim wyświetli się interakcyjne okno dialogowe. Ta strona jest też wyświetlana po wylogowaniu się.

Aplikacja odwołuje się do usługi Microsoft Graph, aby wyświetlać informacje o użytkowniku i jego zdjęcie na stronach NavigationPane i SettingsPage.  Domyślnie funkcja ta działa na kontach w dowolnym katalogu organizacyjnym oraz na osobistych kontach Microsoft (takich jak Skype, Xbox czy Outlook.com). Umożliwia też wykluczenie osobistych kont Microsoft, ograniczenie dostępu do konkretnego katalogu oraz użycie zintegrowanego uwierzytelniania systemu Windows.

[Dowiedz się więcej o platformie Microsoft Identity.](https://docs.microsoft.com/azure/active-directory/develop/v2-overview)
