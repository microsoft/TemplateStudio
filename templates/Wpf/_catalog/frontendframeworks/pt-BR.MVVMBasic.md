**Aviso: O MVVM Basic foi substituído pelo MVVM Toolkit e será removido como opção em uma versão futura do Windows Template Studio.**

Esta é uma versão genérica de um MVVM pattern.  O [padrão Model-View-ViewModel](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel) pode ser usado em todas as plataformas XAML. Sua intenção é fornecer uma separação sem preocupações entre os controles da interface do usuário e sua lógica.

Há três componentes principais no MVVM pattern: o modelo, a exibição e o modelo de exibição. Cada um deles desempenha uma função distinta e separada.

O MVVM Basic não é uma framework, mas fornece a funcionalidade mínima para criar um aplicativo usando o padrão Model-View-ViewModel (MVVM).
Use-o se você não puder ou não quiser usar uma Framework MVVM de terceiros.

O MVVM Basic não se destina a ser uma Framework MVVM com recursos completos e não inclui alguns recursos que outras framework possuem. Navegação, IOC e mensagens de ViewModel-first são os mais óbvios. Se você quiser esses recursos, escolha uma framework que dê suporte a eles.

Projetos criados com MVVM Basic contêm duas classes importantes, `Observable` e` RelayCommand`.
**Observable** contém uma implementação da interface `INotifyPropertyChanged` e é usada como uma classe base para todos os modelos de exibição. Isso facilita a atualização de propriedades vinculadas na Exibição.
**RelayCommand** contém uma implementação da interface `ICommand` para tornar mais fácil ter os comandos de chamada de Exibição no ViewModel, em vez de lidar com os eventos da interface do usuário diretamente.
