Imports System.ComponentModel

Namespace ViewModels
    Public Class BlankViewViewModel
        Implements INotifyPropertyChanged

        Public Sub New()
        End Sub

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    End Class
End Namespace
