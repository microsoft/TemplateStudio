Imports Windows.Devices.Geolocation
Imports Windows.ApplicationModel.Core
Imports Windows.UI.Core
Imports Param_RootNamespace.Helpers

Namespace Services
    Public Class LocationService
        Private geolocator As Geolocator

        Public Event PositionChanged As EventHandler(Of Geoposition)

        Public Property CurrentPosition As Geoposition

        Public Async Function InitializeAsync() As Task(Of Boolean)
            Return Await InitializeAsync(100)
        End Function

        Public Async Function InitializeAsync(desiredAccuracyInMeters As UInteger) As Task(Of Boolean)
            Return Await InitializeAsync(desiredAccuracyInMeters, CDbl(desiredAccuracyInMeters) / 2)
        End Function

        Public Async Function InitializeAsync(desiredAccuracyInMeters As UInteger, movementThreshold As Double) As Task(Of Boolean)
            ' More about getting location at https://docs.microsoft.com/windows/uwp/maps-and-location/get-location
            If geolocator IsNot Nothing Then
                RemoveHandler geolocator.PositionChanged, AddressOf Geolocator_PositionChanged
                geolocator = Nothing
            End If

            Dim access = Await Geolocator.RequestAccessAsync()

            Dim result As Boolean

            Select Case access
                Case GeolocationAccessStatus.Allowed
                    geolocator = New Geolocator() With {
                        .DesiredAccuracyInMeters = desiredAccuracyInMeters,
                        .MovementThreshold = movementThreshold
                    }
                    result = True
                Case Else
                    result = False
            End Select

            Return result
        End Function

        Public Async Function StartListeningAsync() As Task
            If geolocator Is Nothing Then
                Throw New InvalidOperationException("ExceptionLocationServiceStartListeningCanNotBeCalled".GetLocalized())
            End If

            AddHandler geolocator.PositionChanged, AddressOf Geolocator_PositionChanged

            CurrentPosition = Await geolocator.GetGeopositionAsync()
        End Function

        Public Sub StopListening()
            If geolocator Is Nothing Then
                Return
            End If

            RemoveHandler geolocator.PositionChanged, AddressOf Geolocator_PositionChanged
        End Sub

        Private Async Sub Geolocator_PositionChanged(sender As Geolocator, args As PositionChangedEventArgs)
            If args Is Nothing Then
                Return
            End If

            CurrentPosition = args.Position

            Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                RaiseEvent PositionChanged(Me, CurrentPosition)
            End Sub)
        End Sub
    End Class
End Namespace
