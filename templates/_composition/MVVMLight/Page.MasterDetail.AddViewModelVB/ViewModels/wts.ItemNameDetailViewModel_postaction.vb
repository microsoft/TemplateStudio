'{[{
Imports GalaSoft.MvvmLight
Imports GalaSoft.MvvmLight.Command
'}]}
Namespace ViewModels
    Public Class wts.ItemNameDetailViewModel
        Inherits ViewModelBase
        '{[{
        Public ReadOnly Property NavigationService() As NavigationServiceEx
            Get
                Return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of NavigationServiceEx)()
            End Get
        End Property

        '}]}
    End Class
End Namespace
