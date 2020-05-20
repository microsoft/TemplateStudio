È una versione generica di uno schema MVVM pattern.  Lo [schema Model-View-ViewModel](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel) può essere utilizzato su tutte le piattaforme XAML. Intende fornire una separazione chiara dei problemi tra controlli dell'interfaccia utente e relativa logica.

Lo schema MVVM pattern è costituito da tre componenti fondamentali, ovvero modello, visualizzazione e modello di visualizzazione. Ciascuno ha un ruolo separato e distinto.

MVVM Basic non è un framework ma offre le funzionalità minime per creare un'app utilizzando lo schema Model-View-ViewModel (MVVM).
Usalo se non puoi o non desideri utilizzare un Framework MVVM di terze parti.

MVVM Basic non è pensato come Framework MVVM con funzionalità complete e non include alcune funzionalità presenti in altri framework, come l'esplorazione ViewModel-first, IOC e la messaggistica. Se desideri queste funzionalità, scegli un framework che le supporti.

I progetti creati con MVVM Basic contengono due classi importanti, "Observable" e "RelayCommand".
**Observable** contiene un'implementazione dell'interfaccia "INotifyPropertyChanged" ed è usata come classe di base per tutti i modelli di visualizzazione. Ciò semplifica l'aggiornamento delle proprietà associate nella visualizzazione.
**RelayCommand** contiene un'implementazione dell'interfaccia "ICommand" che agevola la presenza dei comandi di visualizzazione chiamata in ViewModel, anziché gestire direttamente gli eventi dell'interfaccia utente.
