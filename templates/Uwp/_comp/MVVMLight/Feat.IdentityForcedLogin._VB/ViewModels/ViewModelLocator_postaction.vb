Namespace ViewModels
    Public Class ViewModelLocator
        Private Sub New()
'^^
'{[{
            Register(Of LogInViewModel, LogInPage)()
'}]}
        End Sub

'{[{
        Public ReadOnly Property LogInViewModel As LogInViewModel
            Get
                Return SimpleIoc.[Default].GetInstance(Of LogInViewModel)()
            End Get
        End Property
'}]}
    End Class
End Namespace
