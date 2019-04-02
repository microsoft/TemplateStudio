İsteğe bağlı oturum açma seçeneği, Azure AD ve [Microsoft.Identity.Client](https://www.nuget.org/packages/Microsoft.Identity.Client) (MSAL) kullanarak kullanıcı kimlik doğrulaması ekler. 
Bu stil, kısıtlamasız ve kısıtlamalı içeriğin birleştirilmesine olanak tanır. Kısıtlamalı içerik yalnızca oturum açmış ve yetkilendirilmiş kullanıcılara gösterilir.
Uygulama, NavigationPane ve SettingsPage üzerinde kullanıcı bilgilerini ve fotoğrafını göstermek için bir Microsoft Graph çağrısı içerir.

Bu özellik varsayılan olarak tüm kurumsal dizinlerdeki Hesaplarla ve kişisel Microsoft hesaplarıyla (örneğin Skype, Xbox, Outlook.com) çalışır. Ayrıca kişisel Microsoft hesaplarını hariç tutma, belirli bir dizine erişimi sınırlama veya tümleşik kimlik doğrulamasını kullanma seçenekleri sunar.

[Microsoft Identity platformu hakkında daha fazla bilgi edinin.](https://docs.microsoft.com/azure/active-directory/develop/v2-overview)
