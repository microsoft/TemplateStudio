Imports Windows.Devices.Geolocation
Imports Windows.Storage.Streams
Imports Windows.UI.Xaml.Controls.Maps
Imports Param_ItemNamespace.Helpers
Imports Param_ItemNamespace.Services

Namespace Views
    Public NotInheritable Partial Class MapPagePage
        Inherits Page
        Implements System.ComponentModel.INotifyPropertyChanged

        ' TODO WTS: Set your preferred default zoom level
        Private Const DefaultZoomLevel As Double = 17

        Private ReadOnly locationService As LocationService

        ' TODO WTS: Set your preferred default location if a geolock can't be found.
        Private ReadOnly defaultPosition As New BasicGeoposition() With {
            .Latitude = 47.609425,
            .Longitude = -122.3417
        }

        Private _zoomLevel As Double

        Public Property ZoomLevel As Double
            Get
                Return _zoomLevel
            End Get
            Set
                [Param_Setter](_zoomLevel, value)
            End Set
        End Property

        Private _center As Geopoint

        Public Property Center As Geopoint
            Get
                Return _center
            End Get
            Set
                [Param_Setter](_center, value)
            End Set
        End Property

        Public Sub New()
            locationService = New LocationService()
            Center = New Geopoint(defaultPosition)
            ZoomLevel = DefaultZoomLevel
            InitializeComponent()
        End Sub

        Public Async Function InitializeAsync() As Task
            If locationService IsNot Nothing Then
                AddHandler locationService.PositionChanged, AddressOf LocationService_PositionChanged

                Dim initializationSuccessful = Await locationService.InitializeAsync()

                If initializationSuccessful Then
                    Await locationService.StartListeningAsync()
                End If

                If initializationSuccessful AndAlso locationService.CurrentPosition IsNot Nothing Then
                    Center = locationService.CurrentPosition.Coordinate.Point
                Else
                    Center = New Geopoint(defaultPosition)
                End If
            End If

            If mapControl IsNot Nothing Then
                ' TODO WTS: Set your map service token. If you don't have one, request at https://www.bingmapsportal.com/
                mapControl.MapServiceToken = String.Empty

                AddMapIcon(Center, "Map_YourLocation".GetLocalized())
            End If
        End Function

        Public Sub Cleanup()
            If locationService IsNot Nothing Then
                RemoveHandler locationService.PositionChanged, AddressOf LocationService_PositionChanged
                locationService.StopListening()
            End If
        End Sub

        Private Sub LocationService_PositionChanged(sender As Object, geoposition As Geoposition)
            If geoposition IsNot Nothing Then
                Center = geoposition.Coordinate.Point
            End If
        End Sub

        Private Sub AddMapIcon(position As Geopoint, title As String)
            Dim mapIcon As New MapIcon() With {
                .Location = position,
                .NormalizedAnchorPoint = New Point(0.5, 1.0),
                .Title = title,
                .Image = RandomAccessStreamReference.CreateFromUri(New Uri("ms-appx:///Assets/map.png")),
                .ZIndex = 0
            }
            mapControl.MapElements.Add(mapIcon)
        End Sub
    End Class
End Namespace
