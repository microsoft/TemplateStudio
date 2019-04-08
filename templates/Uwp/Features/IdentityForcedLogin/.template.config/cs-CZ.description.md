Šablona Forced Login přidává ověřování uživatelů pomocí Azure AD a [Microsoft.Identity.Client](https://www.nuget.org/packages/Microsoft.Identity.Client) (MSAL). 
Přístup k vaší aplikaci je omezený pouze na přihlášené a autorizované uživatele. Je-li požadováno interaktivní přihlášení, uživatel je před zobrazením interaktivního dialogového okna přesměrován na přihlašovací stránku. Stejná stránka se zobrazuje po odhlášení.

Aplikace zahrnuje volání nástroje Microsoft Graph k zobrazení informací o uživateli a jeho fotografie na navigačním panelu a stránce nastavení.  Ve výchozím nastavení tato funkce pracuje s účty v libovolné adresářové službě organizace a s osobními účty Microsoft (např. Skype, Xbox, Outlook.com) a umožňuje vyloučit osobní účty Microsoft, omezit přístup k určitým adresářům a využívat integrované ověřování systému Windows.

[Další informace o platformě Microsoft Identity](https://docs.microsoft.com/azure/active-directory/develop/v2-overview)
