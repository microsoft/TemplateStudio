Namespace Views
    ' TODO WTS: Remove this example page when/if it's not needed.
    ' This page is an example of how to launch a specific page in response to a protocol launch and pass it a value.
    ' It is expected that you will delete this page once you have changed the handling of a protocol launch to meet
    ' your needs and redirected to another of your pages.
    Public NotInheritable Partial Class SchemeActivationSamplePage
        Inherits Page
        Implements INotifyPropertyChanged

        Public ReadOnly Property Parameters As ObservableCollection(Of String) = New ObservableCollection(Of String)()

        Protected Overrides Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
            Dim parameters = TryCast(e.Parameter, Dictionary(Of String, String))
            Initialize(parameters)
        End Sub

        Private Sub Initialize(ByVal parameters As Dictionary(Of String, String))
            Dim ticks As Long = Nothing
            For Each param In parameters

                If param.Key = "ticks" AndAlso Long.TryParse(param.Value, ticks) Then
                    Dim dateTime = New DateTime(ticks)
                    Me.Parameters.Add($"{param.Key}: {dateTime.ToString()}")
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
    End Class
End Namespace
