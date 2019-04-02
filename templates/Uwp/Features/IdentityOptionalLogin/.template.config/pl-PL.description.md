Funkcja opcjonalnego logowania dodaje uwierzytelnianie użytkowników przy użyciu usługi Azure AD i biblioteki [Microsoft.Identity.Client](https://www.nuget.org/packages/Microsoft.Identity.Client) (MSAL). 
Pozwala to na jednoczesne korzystanie z zawartości o różnym poziomie ograniczeń (z restrykcjami lub bez). Zawartość z restrykcjami jest wyświetlana tylko zalogowanym i autoryzowanym użytkownikom.
Aplikacja odwołuje się do usługi Microsoft Graph, aby wyświetlać informacje o użytkowniku i jego zdjęcie na stronach NavigationPane i SettingsPage.

Domyślnie funkcja ta działa na kontach w dowolnym katalogu organizacyjnym oraz na osobistych kontach Microsoft (takich jak Skype, Xbox czy Outlook.com). Umożliwia też wykluczenie osobistych kont Microsoft, ograniczenie dostępu do konkretnego katalogu oraz użycie uwierzytelniania zintegrowanego.

[Dowiedz się więcej o platformie Microsoft Identity.](https://docs.microsoft.com/azure/active-directory/develop/v2-overview)
