Il s'agit d'une version générique d'un MVVM pattern.  Le modèle MVVM [modèle-vue-vue modèle]https://fr.wikipedia.org/wiki/Mod%C3%A8le-vue-vue_mod%C3%A8le) peut être utilisé sur toutes les plateformes XAML. Il est destiné à séparer de manière distincte les éléments relevant des commandes de l'interface utilisateur et leur logique.

Il existe trois composants principaux dans le MVVM pattern : le modèle, la vue et le modèle de vue. Chacun d'eux joue un rôle bien défini.

MVVM Basic n'est pas une framework, mais fournit les fonctionnalités minimales pour créer une application à l'aide du modèle MVVM (modèle-vue-vue modèle).
Utilisez-le si vous ne parvenez ou ne souhaitez pas utiliser une Framework MVVM tierce.

MVVM Basic n'est pas prévu pour prendre en charge l'intégralité des fonctionnalités de la Framework MVVM et n'inclut pas des fonctionnalités que d'autres framework proposent. La navigation ViewModel-first, IOC et la messagerie sont les fonctionnalités les plus évidentes. Si vous souhaitez utiliser ces fonctionnalités, optez pour une framework les prenant en charge.

Les projets créés avec MVVM Basic contiennent deux catégories importantes : « Observable » et « RelayCommand ».
**Observable** contient une mise en application de l'interface « INotifyPropertyChanged » et est utilisée en tant que catégorie de base pour tous les modèles de visualisation. Cela permet de mettre à jour les propriétés associées dans la Vue.
**RelayCommand** contient une mise en place de l'interface « ICommand » pour permettre d'appliquer aisément les commandes d'appel Vue dans le ViewModel plutôt que de gérer les événements d'interface utilisateur directement.
