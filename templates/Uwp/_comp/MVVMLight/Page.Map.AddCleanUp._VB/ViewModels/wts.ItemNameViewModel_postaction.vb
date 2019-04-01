Namespace ViewModels
    Public Class wts.ItemNameViewModel
        Inherits ViewModelBase
        '^^

        '{[{
        Public Overrides Sub Cleanup()
            If locationService IsNot Nothing Then
                RemoveHandler locationService.PositionChanged, AddressOf LocationService_PositionChanged
                locationService.StopListening()
            End If

            MyBase.Cleanup()
        End Sub
        '}]}
    End Class
End Namespace
