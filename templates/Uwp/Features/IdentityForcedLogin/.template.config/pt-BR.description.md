O Forced Login adiciona autenticação do usuário usando o Azure AD e o [Microsoft.Identity.Client](https://www.nuget.org/packages/Microsoft.Identity.Client) (MSAL). 
O acesso ao seu aplicativo é restrito a usuários conectados e autorizados. Se o logon interativo for necessário, o usuário será redirecionado para uma Página de Logon antes de a caixa de diálogo interativa ser exibida. A mesma página é mostrada após o logout.

O aplicativo inclui uma chamada para o Microsoft Graph para mostrar informações e fotos do usuário no Painel de navegação e na Página de configurações.  Por padrão, esses recursos funcionam com contas em qualquer diretório organizacional e contas pessoais da Microsoft (por exemplo, Skype, Xbox, Outlook.com) e oferecem a opção de excluir contas pessoais da Microsoft, limitar o acesso a um diretório específico ou usar autorização integrada ao Windows.

[Saiba mais sobre a plataforma Microsoft Identity.](https://docs.microsoft.com/azure/active-directory/develop/v2-overview)
