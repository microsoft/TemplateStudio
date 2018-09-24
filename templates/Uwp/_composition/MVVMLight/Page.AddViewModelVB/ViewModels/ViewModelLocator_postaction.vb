Namespace ViewModels
    Public Class ViewModelLocator
        Private Sub New()
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
