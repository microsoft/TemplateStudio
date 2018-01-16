Namespace ViewModels
    Public Class ViewModelLocator
        Public Sub New()
            ServiceLocator.SetLocatorProvider(Function() SimpleIoc.[Default])
            '{[{
            If SimpleIoc.[Default].IsRegistered(Of NavigationServiceEx)() Then
                Return
            End If
            '}]}
            SimpleIoc.[Default].Register(Function() New NavigationServiceEx())
            '^^
            '{[{
            Register(Of wts.ItemNameViewModel, wts.ItemNamePage)()
                '}]}
        End Sub

        '{[{

        Public ReadOnly Property wts.ItemNameViewModel As wts.ItemNameViewModel
            Get
                Return ServiceLocator.Current.GetInstance(Of wts.ItemNameViewModel)()
            End Get
        End Property
        '}]}
    End Class
End Namespace
