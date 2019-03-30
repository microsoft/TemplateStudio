Namespace ViewModels
    Public Class ViewModelLocator
        Private Sub New()
            '^^
            '{[{
            SimpleIoc.[Default].Register(Of ShellViewModel)()
            '}]}
        End Sub
        '{[{

        Public ReadOnly Property ShellViewModel As ShellViewModel
            Get
                Return SimpleIoc.[Default].GetInstance(Of ShellViewModel)()
            End Get
        End Property
        '}]}
    End Class
End Namespace
