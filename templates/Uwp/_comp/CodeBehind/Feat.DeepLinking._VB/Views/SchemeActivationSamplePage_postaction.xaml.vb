'{[{
'}]}
Namespace Views
    Public NotInheritable Partial Class SchemeActivationSamplePage
        Inherits Page
'{[{
        Implements INotifyPropertyChanged

        Public ReadOnly Property Parameters As ObservableCollection(Of String) = New ObservableCollection(Of String)()

        Protected Overrides Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
            If e.Parameter IsNot Nothing Then
                Dim parameters = TryCast(e.Parameter, Dictionary(Of String, String))
                Initialize(parameters)
            End If
        End Sub

        Private Sub Initialize(parameters As Dictionary(Of String, String))
            Me.Parameters.Clear()
            Dim ticks As Long = Nothing
            For Each param In parameters

                If param.Key = "ticks" AndAlso Long.TryParse(param.Value, ticks) Then
                    Dim dateTime = New DateTime(ticks)
                    Me.Parameters.Add($"{param.Key}: {dateTime}")
                Else
                    Me.Parameters.Add($"{param.Key}: {param.Value}")
                End If
            Next
        End Sub

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Private Sub [Set](Of T)(ByRef storage As T, value As T, <CallerMemberName> Optional propertyName As String = Nothing)
            If Equals(storage, value) Then
                Return
            End If

            storage = value
            OnPropertyChanged(propertyName)
        End Sub

        Private Sub OnPropertyChanged(propertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub
'}]}
    End Class
End Namespace