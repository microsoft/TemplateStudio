Imports System.Threading.Tasks
Imports Windows.Devices.Geolocation

Namespace Services
    Public Class LocationService
        Private _geolocator As Geolocator

        ''' <summary>
        ''' Raised when the current position is updated.
        ''' </summary>
        Public Event PositionChanged As EventHandler(Of Geoposition)

        ''' <summary>
        ''' Gets the last known recorded position.
        ''' </summary>
        Public Property CurrentPosition As Geoposition

        ''' <summary>
        ''' Initializes the location service with a default accuracy (100 meters) and movement threshold.
        ''' </summary>
        ''' <returns>True if the initialization was successful and the service can be used.</returns>
        Public Function InitializeAsync() As Task(Of Boolean)
            Return InitializeAsync(100)
        End Function

        ''' <summary>
        ''' Initializes the location service with the specified accuracy and default movement threshold.
        ''' </summary>
        ''' <param name="desiredAccuracyInMeters">The desired accuracy at which the service provides location updates.</param>
        ''' <returns>True if the initialization was successful and the service can be used.</returns>
        Public Function InitializeAsync(desiredAccuracyInMeters As UInteger) As Task(Of Boolean)
            Return InitializeAsync(desiredAccuracyInMeters, CDbl(desiredAccuracyInMeters) / 2)
        End Function

        ''' <summary>
        ''' Initializes the location service with the specified accuracy and movement threshold.
        ''' </summary>
        ''' <param name="desiredAccuracyInMeters">The desired accuracy at which the service provides location updates.</param>
        ''' <param name="movementThreshold">The distance of movement, in meters, that is required for the service to raise the PositionChanged event.</param>
        ''' <returns>True if the initialization was successful and the service can be used.</returns>
        Public Async Function InitializeAsync(desiredAccuracyInMeters As UInteger, movementThreshold As Double) As Task(Of Boolean)
            ' to find out more about getting location, go to https://docs.microsoft.com/en-us/windows/uwp/maps-and-location/get-location
            If _geolocator IsNot Nothing Then
                RemoveHandler _geolocator.PositionChanged, AddressOf Geolocator_PositionChanged
                _geolocator = Nothing
            End If
            Dim access = Await Geolocator.RequestAccessAsync()
            Dim result As Boolean
            Select Case access
                Case GeolocationAccessStatus.Allowed
                    _geolocator = New Geolocator With {
                        .desiredAccuracyInMeters = desiredAccuracyInMeters,
                        .movementThreshold = movementThreshold
                    }
                    result = True
                Case Else
                    result = False
            End Select
            Return result
        End Function

        ''' <summary>
        ''' Starts the service listening for location updates.
        ''' </summary>
        ''' <returns>An object that is used to manage the asynchronous operation.</returns>
        Public Async Function StartListeningAsync() As Task
            If _geolocator Is Nothing Then
                Throw New InvalidOperationException("The StartListening method cannot be called before the InitializeAsync method.")
            End If
            AddHandler _geolocator.PositionChanged, AddressOf Geolocator_PositionChanged
            CurrentPosition = Await _geolocator.GetGeopositionAsync()
        End Function

        ''' <summary>
        ''' Stops the service listening for location updates.
        ''' </summary>
        Public Sub StopListening()
            If _geolocator Is Nothing Then
                Return
            End If
            RemoveHandler _geolocator.PositionChanged, AddressOf Geolocator_PositionChanged
        End Sub

        Private Async Sub Geolocator_PositionChanged(sender As Geolocator, args As PositionChangedEventArgs)
            If args Is Nothing Then
                Return
            End If
            CurrentPosition = args.Position
            Await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                      Sub() PositionChanged?.Invoke(Me, CurrentPosition))
        End Sub
    End Class
End Namespace
