A configuração de armazenamento é uma classe para simplificar o armazenamento de dados do aplicativo.  Lida com o carregamento, o salvamento, a serialização dos dados e o acesso fácil aos dados do aplicativo.

Estes são os principais tipos de dados de aplicativo:

* Locais: armazenados no dispositivo, com backup feito na nuvem e persistem entre atualizações
* LocalCache: dados persistentes que existem no dispositivo atual, sem backup e persistem entre atualizações
* SharedLocal: persistentes entre todos os usuários do aplicativo
* Roaming: existem em todos os dispositivos em que o usuário instalou o aplicativo
* Temporários: podem ser excluídos pelo sistema a qualquer momento

Para saber mais sobre armazenamento, acesse [docs.microsoft.com](https://docs.microsoft.com/en-us/uwp/api/windows.storage.applicationdata).
