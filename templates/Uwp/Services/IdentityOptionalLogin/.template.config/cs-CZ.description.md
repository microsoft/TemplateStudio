Volitelné přihlašování přidává ověřování uživatelů pomocí Azure AD a [Microsoft.Identity.Client](https://www.nuget.org/packages/Microsoft.Identity.Client) (MSAL).
Tento styl také umožňuje kombinovat neomezený a omezený obsah. Omezený obsah se zobrazuje pouze přihlášeným a autorizovaným uživatelům.
Aplikace zahrnuje volání nástroje Microsoft Graph k zobrazení informací o uživateli a jeho fotografie na navigačním panelu a stránce nastavení.

Ve výchozím nastavení tato funkce pracuje s účty v libovolné adresářové službě organizace a s osobními účty Microsoft (např. Skype, Xbox, Outlook.com) a umožňuje vyloučit osobní účty Microsoft, omezit přístup k určitým adresářům a využívat integrované ověřování.

[Další informace o platformě Microsoft Identity](https://docs.microsoft.com/azure/active-directory/develop/v2-overview)
