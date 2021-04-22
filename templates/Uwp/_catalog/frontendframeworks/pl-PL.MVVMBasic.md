**Uwaga: MVVM Basic został zastąpiony przez MVVM Toolkit i zostanie usunięty jako opcja w przyszłej wersji Windows Template Studio.**

To jest ogólna wersja wzorca MVVM.  Wzorzec [Model-View-ViewModel](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel) może być używany na wszystkich platformach XAML. Pozwala on wyraźnie oddzielić problemy związane z kontrolkami interfejsu użytkownika od tych dotyczących ich logiki.

Wzorzec MVVM składa się z trzech głównych komponentów: model, view i view model. Każdy spełnia oddzielną rolę.

MVVM Basic to nie frameworkiem, ale oferuje minimalną funkcjonalność do tworzenia aplikacji za pomocą wzorca Model-View-ViewModel (MVVM).
Skorzystaj z tej opcji, jeśli nie możesz lub nie chcesz używać zewnętrznego Frameworku MVVM.

MVVM Basic nie oferuje wszystkich funkcji Frameworku MVVM i nie zawiera niektórych funkcji dostępnych w innych frameworkach. Najbardziej oczywiste z nich, to nawigacja ViewModel-first, IOC oraz obsługa wiadomości. Jeśli chcesz korzystać z tych funkcji, wybierz framework, który je obsługuje.

Projekty tworzone w MVVM Basic zawierają dwie ważne klasy, „Observable” i „RelayCommand”.
Klasa „Observable” zawiera implementację interfejsu „INotifyPropertyChanged” i jest używana jako podstawowa klasa do wszystkich modeli view. Ułatwia to aktualizację powiązanych własności w komponencie View.
„RelayCommand” zawiera implementację interfejsu „ICommand”, aby ułatwić wywoływanie poleceń przez komponent View w komponencie ViewModel, zamiast obsługi zdarzeń interfejsu użytkownika w sposób bezpośredni.
