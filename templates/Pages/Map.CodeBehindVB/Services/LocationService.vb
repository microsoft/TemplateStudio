Imports System.Threading.Tasks
Imports Windows.Devices.Geolocation
Imports Param_ItemNamespace.Helpers

Namespace Services
    Public Class LocationService
        Private geolocator As Geolocator

        ''' <summary>
        ''' Raised when the current position is updated.
        ''' </summary>
        Public Event PositionChanged As EventHandler(Of Geoposition)

        ''' <summary>
        ''' Gets the last known recorded position.
        ''' </summary>
        Public Property CurrentPosition As Geoposition
            Get
                Return m_CurrentPosition
            End Get
            Private Set
                m_CurrentPosition = Value
            End Set
        End Property

        Private m_CurrentPosition As Geoposition

        ''' <summary>
        ''' Initializes the location service with a default accuracy (100 meters) and movement threshold.
        ''' </summary>
        ''' <returns>True if the initialization was successful and the service can be used.</returns>
        Public Async Function InitializeAsync() As Task(Of Boolean)
            Return Await InitializeAsync(100)
        End Function

        ''' <summary>
        ''' Initializes the location service with the specified accuracy and default movement threshold.
        ''' </summary>
        ''' <param name="desiredAccuracyInMeters">The desired accuracy at which the service provides location updates.</param>
        ''' <returns>True if the initialization was successful and the service can be used.</returns>
        Public Async Function InitializeAsync(desiredAccuracyInMeters As UInteger) As Task(Of Boolean)
            Return Await InitializeAsync(desiredAccuracyInMeters, CDbl(desiredAccuracyInMeters) / 2)
        End Function

        ''' <summary>
        ''' Initializes the location service with the specified accuracy and movement threshold.
        ''' </summary>
        ''' <param name="desiredAccuracyInMeters">The desired accuracy at which the service provides location updates.</param>
        ''' <param name="movementThreshold">The distance of movement, in meters, that is required for the service to raise the PositionChanged event.</param>
        ''' <returns>True if the initialization was successful and the service can be used.</returns>
        Public Async Function InitializeAsync(desiredAccuracyInMeters As UInteger, movementThreshold As Double) As Task(Of Boolean)
            ' to find out more about getting location, go to https://docs.microsoft.com/en-us/windows/uwp/maps-and-location/get-location
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
                    Exit Select
                ' Case GeolocationAccessStatus.Unspecified, GeolocationAccessStatus.Denied
                Case Else
                    result = False
                    Exit Select
            End Select

            Return result
        End Function

        ''' <summary>
        ''' Starts the service listening for location updates.
        ''' </summary>
        ''' <returns>An object that is used to manage the asynchronous operation.</returns>
        Public Async Function StartListeningAsync() As Task
            If geolocator Is Nothing Then
                Throw New InvalidOperationException("ExceptionLocationServiceStartListeningCanNotBeCalled".GetLocalized())
            End If

            AddHandler geolocator.PositionChanged, AddressOf Geolocator_PositionChanged

            CurrentPosition = Await geolocator.GetGeopositionAsync()
        End Function

        ''' <summary>
        ''' Stops the service listening for location updates.
        ''' </summary>
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

            Await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, Sub() 
                RaiseEvent PositionChanged(Me, CurrentPosition)
            End Sub)
        End Sub
    End Class
End Namespace