Namespace ViewModels
    Public Class ViewModelLocator
        Public Sub New()
            '^^
            '{[{
            Register(Of wts.ItemNameExampleViewModel, wts.ItemNameExamplePage)()
            '}]}
        End Sub

        '{[{

        Public ReadOnly Property wts.ItemNameExampleViewModel As wts.ItemNameExampleViewModel
            Get
                Return ServiceLocator.Current.GetInstance(Of wts.ItemNameExampleViewModel)
            End Get
        End Property
        '}]}
    End Class
End Namespace
