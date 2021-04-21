**Upozornění: MVVM Basic byl nahříván souborem nástrojů MVVM Toolkit a bude odebrán jako možnost v budoucí verzi sady Windows Template Studio.**

Toto je obecná verze vzoru MVVM pattern.  [Vzor Model-View-ViewModel](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel) lze používat na všech platformách XAML. Jeho záměrem je poskytovat jasné rozdělení rozsahu působnosti mezi ovládacími prvky uživatelského rozhraní a jejich logikou.

Vzor MVVM pattern obsahuje tři základní součásti: model, zobrazení a model zobrazení. Každá z těchto součástí má odlišnou a samostatnou roli.

MVVM Basic není rámec (framework), ale poskytuje minimální sadu funkcí pro vytvoření aplikace pomocí vzoru Model-View-ViewModel (MVVM).
Používá se, pokud nemůžete nebo nechcete použít rámec (framework) MVVM třetí strany.

Cílem řešení MVVM Basic není být plnohodnotným rámcem (framework) MVVM, a proto neobsahuje některé funkce obsažené v dalších rámcích (framework). Mezi tyto funkce patří především navigace typu ViewModel-first, přerušení při dokončení (IOC) a zasílání zpráv. Pokud tyto funkce vyžadujete, zvolte rámec (framework), který je podporuje.

Projekty vytvořené pomocí řešení MVVM Basic obsahují dvě důležité třídy: Observable a RelayCommand.
Třída **Observable** obsahuje implementaci rozhraní INotifyPropertyChanged a používá se jako základní třída pro všechny modely zobrazení. Díky tomu lze snadno aktualizovat vázané vlastnosti v zobrazení.
Třída **RelayCommand** obsahuje implementaci rozhraní ICommand, které usnadňuje umístění příkazů volání zobrazení do modelu zobrazení, namísto aby byly události uživatelského rozhraní zpracovávány přímo.
