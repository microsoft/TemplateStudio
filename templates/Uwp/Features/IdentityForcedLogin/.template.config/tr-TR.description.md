Forced Login seçeneği, Azure AD ve [Microsoft.Identity.Client](https://www.nuget.org/packages/Microsoft.Identity.Client) (MSAL) kullanarak kullanıcı kimlik doğrulaması ekler. 
Uygulamanıza erişim, oturum açmış ve yetkilendirilmiş kullanıcılarla sınırlıdır. Etkileşimli oturum açma gerekiyorsa etkileşimli iletişim kutusu gösterilmeden önce kullanıcı LoginPage sayfasına yönlendirilir. Oturum kapatıldıktan sonra da aynı sayfa gösterilir.

Uygulama, NavigationPane ve SettingsPage üzerinde kullanıcı bilgilerini ve fotoğrafını göstermek için bir Microsoft Graph çağrısı içerir.  Bu özellik varsayılan olarak tüm kurumsal dizinlerdeki hesaplarla ve kişisel Microsoft hesaplarıyla (örneğin Skype, Xbox, Outlook.com) çalışır. Ayrıca kişisel Microsoft hesaplarını hariç tutma, belirli bir dizine erişimi sınırlama veya Windows tümleşik kimlik doğrulamasını kullanma seçenekleri sunar.

[Microsoft Identity platformu hakkında daha fazla bilgi edinin.](https://docs.microsoft.com/azure/active-directory/develop/v2-overview)
