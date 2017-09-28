'{[{
Imports GalaSoft.MvvmLight.Command
'}]}
Namespace ViewModels
    Public Class wts.ItemNameViewModel
        Inherits ViewModelBase
        '{[{
        Public ReadOnly Property NavigationService() As NavigationServiceEx
            Get
                Return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of NavigationServiceEx)()
            End Get
        End Property
        '}]}

        Private Sub OnItemClick(args As ItemClickEventArgs)
            '{[{
            Dim item As SampleOrder = TryCast(args?.ClickedItem, SampleOrder)
            If item IsNot Nothing Then
                If _currentState.Name = NarrowStateName Then
                    NavigationService.Navigate(GetType(wts.ItemNameDetailViewModel).FullName, item)
                Else
                    Selected = item
                End If
            End If
            '}]}
        End Sub
    End Class
End Namespace
