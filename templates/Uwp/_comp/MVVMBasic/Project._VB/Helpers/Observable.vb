Namespace Helpers
    Public Class Observable
        Implements INotifyPropertyChanged

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Friend Sub [Set](Of T)(ByRef storage As T, newValue As T, <CallerMemberName> Optional propertyName As String = Nothing)
            If Equals(storage, newValue) Then
                return
            End If

            storage = newValue
            OnPropertyChanged(propertyName)
        End Sub

        Protected Sub OnPropertyChanged(propertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub
    End Class
End Namespace
