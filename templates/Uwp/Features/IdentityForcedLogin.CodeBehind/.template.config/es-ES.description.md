Forced Login añade autenticación de usuario con Azure AD y [Microsoft.Identity.Client](https://www.nuget.org/packages/Microsoft.Identity.Client) (MSAL).
El acceso a tu aplicación estará restringido a usuarios autorizados que hayan iniciado sesión. Si se requiere un inicio de sesión interactivo, se redirige al usuario a LoginPage antes de mostrar el cuadro de diálogo interactivo. Se muestra la misma página después de cerrar sesión.

La aplicación incluye una llamada a Microsoft Graph para mostrar la información y la fotografía del usuario en el NavigationPane y la SettingsPage.  Esto funciona de manera predeterminada con cuentas de cualquier directorio de la organización y cuentas personales de Microsoft (por ejemplo, de Skype, Xbox o Outlook.com) y ofrece la opción de excluir las cuentas personales de Microsoft, limitar el acceso a un directorio específico o utilizar la autenticación integrada de Windows.

[Más información sobre la plataforma Microsoft Identity.](https://docs.microsoft.com/azure/active-directory/develop/v2-overview)
