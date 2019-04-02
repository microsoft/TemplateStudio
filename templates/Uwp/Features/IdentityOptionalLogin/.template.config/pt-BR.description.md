O logon opcional adiciona autenticação do usuário usando o Azure AD e o [Microsoft.Identity.Client](https://www.nuget.org/packages/Microsoft.Identity.Client) (MSAL). 
Esse estilo permite combinar conteúdo irrestrito e restrito. O conteúdo restrito só é exibido a usuários conectados e autorizados.
O aplicativo inclui uma chamada para o Microsoft Graph para mostrar informações e fotos do usuário no Painel de navegação e na Página de configurações.

Por padrão, esses recursos funcionam com Contas em qualquer diretório organizacional e contas pessoais da Microsoft (por exemplo, Skype, Xbox, Outlook.com) e oferecem a opção de excluir contas pessoais da Microsoft, limitar o acesso a um diretório específico ou usar autorização integrada ao Windows.

[Saiba mais sobre a plataforma Microsoft Identity.](https://docs.microsoft.com/azure/active-directory/develop/v2-overview)
