Imports System.Threading.Tasks
Imports System.Windows.Input
Imports System.ComponentModel
Imports Windows.Devices.Geolocation
Imports Windows.Foundation
Imports Windows.Storage.Streams
Imports Windows.UI.Xaml.Controls.Maps
Imports Param_ItemNamespace.Helpers
Imports Param_ItemNamespace.Services

Namespace ViewModels
    Public Class MapPageViewModel
        Implements INotifyPropertyChanged

        ' TODO WTS: Set your preferred default zoom level
        Private Const DefaultZoomLevel As Double = 17

        Private ReadOnly _locationService As LocationService

        ' TODO WTS: Set your preferred default location if a geolock can't be found.
        Private ReadOnly _defaultPosition As New BasicGeoposition With {
            .Latitude = 47.609425,
            .Longitude = -122.3417
        }

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Dim _zoomLevel As Double
        Public Property ZoomLevel As Double
            Get
                Return _zoomLevel
            End Get
            Set(value As Double)
                _zoomLevel = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(ZoomLevel)))
            End Set
        End Property

        Dim _center As Geopoint
        Public Property Center As Geopoint
            Get
                Return _center
            End Get
            Set(value As Geopoint)
                _center = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Center)))
            End Set
        End Property

        Public Sub New()
            _locationService = New LocationService()
            Center = New Geopoint(_defaultPosition)
            ZoomLevel = DefaultZoomLevel
        End Sub

        Public Function InitializeAsync(map As MapControl) As Task
            If _locationService IsNot Nothing Then
                AddHandler _locationService.PositionChanged, AddressOf LocationService_PositionChanged

                Dim initializationSuccessful = Await _locationService.InitializeAsync()

				If initializationSuccessful Then
                    Await _locationService.StartListeningAsync()
                End If

                If initializationSuccessful AndAlso _locationService.CurrentPosition IsNot Nothing Then
                    Center = _locationService.CurrentPosition.Coordinate.Point
                Else
                    Center = New Geopoint(_defaultPosition)
                End If
            End If

            If map IsNot Nothing Then
                ' TODO WTS: Set your map service token. If you don't have one, request at https://www.bingmapsportal.com/
                map.MapServiceToken = String.Empty

                AddMapIcon(map, Center, "Map_YourLocation".GetLocalized())
            End If
        End Function

        Private Sub LocationService_PositionChanged(sender As Object, geoposition As Geoposition)
            If geoposition IsNot Nothing Then
                Center = geoposition.Coordinate.Point
            End If
        End Sub

        Private Sub AddMapIcon(map As MapControl, position As Geopoint, title As String)
            Dim mapIcon = New MapIcon With {
                .Location = position,
                .NormalizedAnchorPoint = New Point(0.5, 1.0),
                .title = title,
                .Image = RandomAccessStreamReference.CreateFromUri(New Uri("ms-appx:///Assets/map.png")),
                .ZIndex = 0
            }
            map.MapElements.Add(mapIcon)
        End Sub

    End Class
End Namespace
