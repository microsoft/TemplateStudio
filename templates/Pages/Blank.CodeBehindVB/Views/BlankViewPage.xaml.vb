Imports System.ComponentModel

Namespace Views
    NotInheritable Class BlankViewPage
        Inherits Page
        Implements INotifyPropertyChanged

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    End Class
End Namespace
