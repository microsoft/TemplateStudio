Namespace ViewModels
    Public Class ViewModelLocator
        Private Sub New()
            '^^
            '{[{
            Register(Of wts.ItemNameDetailViewModel, wts.ItemNameDetailPage)()
            '}]}
        End Sub

        '{[{

        Public ReadOnly Property wts.ItemNameDetailViewModel As wts.ItemNameDetailViewModel
            Get
                Return SimpleIoc.[Default].GetInstance(Of wts.ItemNameDetailViewModel)()
            End Get
        End Property
        '}]}
    End Class
End Namespace
