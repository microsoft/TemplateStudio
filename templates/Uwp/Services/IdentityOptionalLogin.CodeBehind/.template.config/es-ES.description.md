El inicio de sesión opcional añade autenticación de usuario con Azure AD y [Microsoft.Identity.Client](https://www.nuget.org/packages/Microsoft.Identity.Client) (MSAL).
Este estilo permite combinar contenido restringido y no restringido. El contenido restringido solo se muestra a usuarios autorizados que hayan iniciado sesión.
La aplicación incluye una llamada a Microsoft Graph para mostrar la información y la fotografía del usuario en el NavigationPane y la SettingsPage.

Esto funciona de manera predeterminada con cuentas de cualquier directorio de la organización y cuentas personales de Microsoft (por ejemplo, de Skype, Xbox o Outlook.com) y ofrece la opción de excluir las cuentas personales de Microsoft, limitar el acceso a un directorio específico o utilizar la autenticación integrada.

[Más información sobre la plataforma Microsoft Identity.](https://docs.microsoft.com/azure/active-directory/develop/v2-overview)
