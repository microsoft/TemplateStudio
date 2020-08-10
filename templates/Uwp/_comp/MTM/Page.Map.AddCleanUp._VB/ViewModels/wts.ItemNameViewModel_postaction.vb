Namespace ViewModels
    Public Class wts.ItemNameViewModel
        Inherits ObservableObject
        '^^
        '{[{

        Public Sub Cleanup()
            If locationService IsNot Nothing Then
                RemoveHandler locationService.PositionChanged, AddressOf LocationService_PositionChanged
                locationService.StopListening()
            End If
        End Sub
        '}]}
    End Class
End Namespace
