Imports System.ComponentModel

Namespace Views
    Partial NotInheritable Class BlankViewPage
        Inherits Page
        Implements INotifyPropertyChanged

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    End Class
End Namespace
