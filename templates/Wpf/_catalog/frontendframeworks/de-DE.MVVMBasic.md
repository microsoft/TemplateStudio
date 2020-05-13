Dies ist eine generische Version eines MVVM pattern.  Das [Model-View-ViewModel-Muster](https://de.wikipedia.org/wiki/Model_View_ViewModel) kann auf allen XAML-Plattformen verwendet werden. Es soll eine klare Trennung zwischen den Steuerelementen der Benutzeroberfläche und ihrer Logik sicherstellen.

Im MVVM pattern gibt es drei Kernkomponenten: Modell, Ansicht und Ansichtmodell. Jede Komponente hat eine andere und eigene Rolle.

MVVM Basic ist kein framework, sondern bietet die Mindestfunktionalität, die für die Erstellung einer App mittels des Model-View-ViewModel (MVVM)-Musters erforderlich ist.
Verwenden Sie das Modell, wenn Sie kein MVVM-Framework eines Drittanbieters verwenden können oder möchten.

MVVM Basic ist nicht als vollständig funktionales MVVM-Framework gedacht und enthält einige Features nicht, die andere frameworks aufweisen. Die prioritäre ViewModel-Navigation, IOC und Messaging sind hier vor allem zu nennen. Wenn Sie diese Features verwenden möchten, wählen Sie ein framework, das diese unterstützt.

Projekte, die in MVVM Basic erstellt werden, enthalten zwei wichtige Klassen, Observable und RelayCommand.
**Observable** enthält eine Implementierung der INotifyPropertyChanged-Schnittstelle und wird als Basisklasse für alle Ansichtmodelle verwendet. Auf diese Weise wird es einfach, gebundene Eigenschaften für View zu aktualisieren.
**RelayCommand** enthält eine Implementierung der ICommand-Schnittstelle, um Befehle für View-Aufrufe für das ViewModel verwenden zu können, statt Benutzeroberflächenelemente direkt zu verarbeiten.
